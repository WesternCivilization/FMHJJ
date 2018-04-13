/// <reference path="../../jquery-1.7.2.min.js" />
/*
依赖:目前确认jquery1.7
版本:jquery.subgrid.1.2.2
*/

/*日期格式化函数
*@param {x} 需要格式化的日期,例如：new Date()
*@param {y} 需要显示的格式，例如yyyy-MM-dd hh:mm:ss
*/
function formatDate(x, y) {
    var z = { M: x.getMonth() + 1, d: x.getDate(), h: x.getHours(), m: x.getMinutes(), s: x.getSeconds() };
    y = y.replace(/(M+|d+|h+|m+|s+)/g, function (v) { return ((v.length > 1 ? "0" : "") + eval('z.' + v.slice(-1))).slice(-2) });
    return y.replace(/(y+)/g, function (v) { return x.getFullYear().toString().slice(-v.length) });
};
//textarea内容变化高度随之变化
function resetheight(obj) {
    if ($(obj).prop("tagName").toLowerCase() == "textarea") {
        $(obj).height(obj.scrollHeight);
    }
}
/*
* Jquery.subgrid.js
* this plugin only used for uceip sub grid
* you must refrence jquery.js before this
* example:var subGrid = $("#subgrid").subgrid({}).data('subgrid');
*/
(function ($) {
    /**
    * jQuery subgrid plugin
    * @param {object|string} options
    * @returns {object} jQuery object
    */
    $.fn.subgrid = function (options) {
        var opts = $.extend({}, $.fn.subgrid.defaults, options);
        return this.each(function () {
            var $this = $(this);
            $this.data('subgrid',
                new $.SubGrid($this, $.meta ? $.extend({}, opts, $this.data()) : opts));
        });
    };
    /**
    * Store default options
    * @type {object}
    */
    $.fn.subgrid.defaults = {
        idField: 'id',
        data: [],
        columns: [],
        Sn: true,
        checkBox: true,
        rowNumber: false,
        gridClass: 'subgrid',
        rowCommon: {
            del: false,
            delfun: null
        }
    };
    /**
    * log the debug in console window
    * @param {string} result
    */
    function debug(message) {
        if (window.console && window.console.log) {
            window.console.log('subgrid: ' + message);
        }
    }
    /**
    * get json object key&value
    */
    function getKeyValue(obj) {
        for (var key in obj) {
            if (obj.hasOwnProperty(key)) {
                return { key: key, value: obj[key] };
            }
        }
    }
    /**
    * SubGrid 类
    * @参数 {object} $elem 指定页面table元素的jquery对象形式
    * @参数 {object} options 指定影响创建subgrid的配置列表
    * @constructor
    */
    $.SubGrid = function ($elem, options) {
        // 检查参数
        if (!$elem || !($elem instanceof $) || $elem.length !== 1 || $elem.get(0).tagName.toUpperCase() !== 'TABLE') {
            throw new Error('无效的参数，必须使用table标签定义表格![subgrid]');
        }
        // 存储当前对象实例的引用
        var _instance = this;
        // 配置选项
        this.options = options;
        // 保存列配置信息
        this.cols = {};
        // 初始化 DOM 元素
        this.dom = {};
        // 存储创建subgrid实例的tale元素
        this.dom.$grid = $elem;

        this.dom.$grid.attr('cellSpacing', 0)
                      .attr('cellPadding', 0)
                      .addClass(this.options.gridClass)
                      .empty();
        // 遍历解析列配置信息到cols
        for (var i = this.options.columns.length; i--;) {
            var col = this.options.columns[i];
            _instance.cols[col.field] = col;
        }

        // 创建表头
        _createRowHeader(this.options);

        // 创建表体
        if (this.options.data) {
            _appendRow(this.options.data);
        }

        // 创建表头
        function _createRowHeader(options) {
            var opts = options;
            var grid = _instance.dom.$grid;
            var rowHeader = $('<tr></tr>').addClass('subgrid_header');
            if (opts.checkBox === true) {
                $('<th scope="col"><input type="checkbox" /></th>')
                    .addClass('sg_pd3')
                    .css('text-align', 'center')
                    .css('width', 35)
                    .appendTo(rowHeader);
                $('[type=checkbox]', rowHeader).click(function () {
                    var checked = $(this).is(':checked');
                    $('.subgrid_checkbox', grid).each(function () {
                        $(this).attr('checked', checked);
                    });
                });
            }
            if (opts.rowNumber === true) {
                $('<th scope="col">序号</th>').width(15).appendTo(rowHeader);
            }
            $.each(opts.columns, function (i, col) {
                if (col.hidden === true) {

                }
                else {
                    $('<th></th>').attr('scope', 'col')
                    //.attr('field', col.field)
                              .addClass('sg_pd3')
                              .css('width', col.width || '100%')
                              .css('text-align', col.align || 'left')
                              .text(col.title)
                              .appendTo(rowHeader);
                }                
            });
            //添加行操作--删除
            if (opts.rowCommon.del === true) {
                $('<th scope="col"></th>').addClass("subgird_Common").appendTo(rowHeader);
            }
            grid.append(rowHeader);
        };

        // 添加行
        function _appendRow(data) {
            var grid = _instance.dom.$grid;
            var opts = _instance.options;
            var rowIndex = grid.find('.subgrid_row').length;
            if (typeof data == "undefined") {
                alert("新增行没有初始化,不能增加行");
                return;
            }
            // 兼容数据集合填充
            var arrayData = $.isArray(data) ? data : [data];
            // 循环添加每一行数据 
            $.each(arrayData, function (index, rowData) {
                rowIndex++;
                var row = $('<tr></tr>').addClass('subgrid_row').appendTo(grid);
                //存储数据到缓存
                row.data('rowData', rowData);
                //添加选择框
                if (opts.checkBox === true) {
                    $('<td class="sg_pd3" style="text-align:center"><input class="subgrid_checkbox" type="checkbox" /></td>').appendTo(row);
                }
                //添加行号
                if (opts.rowNumber === true) {
                    $('<td class="sn">' + rowIndex + '</td>').width(15).appendTo(row);
                }
                //添加数据
                if (opts.columns && opts.columns.length) {
                    $.each(opts.columns, function (index, col) {
                        //col默认值
                        var coldefault = {
                            edit: true,
                            cls: "subgrid_field",
                            align: "left"
                        }
                        //给col设置值
                        $.extend(coldefault, col);
                        col = coldefault;
                        //判断列是否可见
                        var cellcontainer = $('<td' + ((col.hidden === true) ? ' style="display:none"' : '') + '></td>').appendTo(row);
                        
                        //判断列控件类型
                        var html = '<input type="text" warp="Virtual"/>';
                        if (col.input) {
                            html = col.input;                            
                        }
                        
                        // 创建编辑框
                        var input = $(html)
                        //设置列控件字段
                        input.attr("field", col.field);
                        //设置列控件name属性
                        //if (col.type === 'file') {
                        //    input.attr("name", col.field);
                        //}
                        //else {
                            
                        //}
                        input.attr("name", "[" + (rowIndex - 1) + "]." + col.field);

                        //设置列控件id
                        if (col.suf) {
                            //有后缀
                            input.attr("id", col.field + '_' + rowIndex + col.suf);
                        }
                        else {
                            //无后缀
                            input.attr("id", col.field + '_' + rowIndex);
                        }
                        //设置列控件样式
                        input.attr("class", col.cls);
                        //设置列控件是否可以编辑
                        if (!col.edit) {
                            input.attr("disabled", "disabled");
                        }
                        //设置列控件对齐方式
                        input.css('text-align', col.align);
                        //设置编辑框属性                        
                        input.appendTo(cellcontainer)
                                .css('width', '100%')
                                .blur(function (e) {
                                    e.preventDefault();
                                    // 必填验证
                                    if (col.required && !this.value && this.value.length < 1) {
                                        $(this).parent().addClass('invalid'); return false;
                                    } else {
                                        $(this).parent().removeClass('invalid');
                                    }
                                    //验证处理
                                    if ($.isFunction(col.validator)) {
                                        if (col.validator(this) === false) {
                                            $(this).parent().addClass('invalid'); return false;
                                        } else {
                                            $(this).parent().removeClass('invalid');
                                        }
                                    }
                                    //格式化处理
                                    if ($.isFunction(col.formator)) {
                                        this.value = col.formator(this.value);
                                    }

                                    return false;
                                })
                                .focus(function () {
                                    //焦点处理
                                    if ($.isFunction(col.focus)) {
                                        col.focus(this.value);
                                    }
                                })
                                .change(function (e) {
                                    e.preventDefault();
                                    if (col.calc && col.calc.length) {
                                        // 处理连带计算
                                        _handleCalc(this, col.calc);
                                    }
                                    return false;
                                })
                                .keydown(function () { resetheight(this); })
                                .click(function () { resetheight(this); })
                                .keyup(function () { resetheight(this); });
                        // 设置只读
                        if (col.readonly === true) {
                            input.attr('readonly', 'readonly');
                        }
                        // 使用快速检索，该节可以根据不同快速解决方案酌情修改
                        // if (!col.readonly && col.editor && input.autocomplete) {
                        if (col.editor && input.autocomplete) {
                            var editoroptions = $.extend({
                                minChars: 0,
                                max: 10,
                                delay: 200,
                                mustMatch: true,
                                matchContains: false,
                                matchCase: false,
                                autoFill: false
                            }, col.editor.option);
                            input.autocomplete(editoroptions.url, editoroptions).result(function (event, callBackData) {
                                if (typeof (editoroptions.onItemSelected) == 'function') {
                                    editoroptions.onItemSelected(event, callBackData);
                                }
                                if (editoroptions.cellFill) {
                                    _handleCellFill(this, editoroptions.cellFill, callBackData);
                                }
                            });
                        }
                        // 绑定传入事件
                        if (col.event) {
                            input.bind(col.event.evt, function () { col.event.fun(row, input); });
                        }
                        //列html改变
                        if ($.isFunction(col.init)) {
                            col.init(input, row);
                        }
                        // 设置初始值
                        if (rowData) {
                            var oldvalue = rowData[col.field];
                            if ($.isFunction(col.formator)) {
                                input.val(col.formator(oldvalue));
                            } else {
                                if (oldvalue != null && oldvalue != "undefind" && oldvalue != "null") {
                                    input.val(oldvalue);
                                }
                            }
                        }
                    }); // end each columns
                } // end if columns

                //添加行操作--删除
                if (opts.rowCommon.del === true) {
                    var vale = row.data('rowData')[opts.idField];
                    if ($.isFunction(opts.rowCommon.delfun)) {
                        $('<td class="subgrid_del" title="删除" >删除</td>').click(function () { opts.rowCommon.delfun(vale, row); }).appendTo(row);
                    }
                }
            }); // end each row
        };

        // 获取所有选中的行
        function _getSelectedRows() {
            var selectedRows = [];
            $('.subgrid_checkbox:checked', _instance.dom.$grid).each(function (index, item) {
                selectedRows.push($(this).parent().parent());
            });
            return selectedRows;
        }

        // 自动填充单元格

        function _handleCellFill(sender, cells, data) {
            if (!sender || !cells || !data || !cells.length) {
                return;
            }
            var row = $(sender).parent().parent();
            for (var i = 0; i < cells.length; i++) {
                var cellKeyValue = getKeyValue(cells[i]);
                if (!cellKeyValue) continue;
                var input = row.find('[field="' + cellKeyValue.key + '"]');
                var dataval = data[cellKeyValue.value];
                if (dataval == null || dataval == "null") {
                    dataval = "";
                }
                if (input && input.length) {
                    input[0].value = dataval;
                } else {
                    $('<input type="hidden" name="' + cellKeyValue.key + '" field="' + cellKeyValue.key + '" value="' + dataval + '" />').appendTo($(sender).parent());
                }
            }
        }

        // 处理连带计算
        function _handleCalc(sender, args) {
            var row = $(sender).parent().parent();
            for (var i = 0; i < args.length; i++) {
                var target = row.find('[field="' + args[i].target + '"]'), //所影响的目标字段
                                    expression = args[i].exp, //计算表达式
                                    fields = args[i].fields; // 参与计算的字段
                for (var j = 0; j < fields.length; j++) {
                    var fieldValue = row.find('[field="' + fields[j] + '"]').val();
                    expression = expression.replace(fields[j], (fieldValue && fieldValue !== NaN) ? fieldValue : 0);
                }
                target.val(eval(expression).toFixed(2));
            }
        }

        //重新排列序号
        function refreshSn() {
            $("#" + $elem.attr("id") + " .sn").each(function (i) {
                $(this).html(i + 1);
            });
        }

        return {
            //重新排列序号
            refreshRowNo: function () {
                refreshSn();
            },
            // 增加一行
            // 传入参数value为该行的默认数据（json形式）
            // 如果想要增加多行，请传入json数组
            addRow: function (value) {
                _appendRow(value);
            },
            // 删除当前选中的行
            delRow: function (setting) {
                var defaultsetting = {
                    isconfirm: true
                };
                $.extend(defaultsetting, setting);
                var selectedrows = _getSelectedRows();
                if (selectedrows.length < 1) {
                    alert("请先选择需要删除的行!");
                    return;
                }
                if (defaultsetting.isconfirm) {
                    if (confirm("确定要删除所选行吗？")) {
                        $.each(selectedrows, function (i, row) {
                            row.removeData('rowData').remove();
                        });
                    }
                }
                else {
                    $.each(selectedrows, function (i, row) {
                        row.removeData('rowData').remove();
                    });
                }
                refreshSn();
            },
            delAllRow: function () {
                $('.subgrid_row', _instance.dom.$grid).each(function (index, row) {
                    var row = $(this);
                    row.remove();
                });
            },
            // 获取当前选中的行，包括隐藏字段
            getSelectedRows: function () {
                var rows = [], selectedrows = _getSelectedRows();
                $.each(selectedrows, function (index, row) {
                    var row = $(this),
                        rowData = row.data('rowData') || {};
                    row.find('[field]').each(function (i, input) {
                        rowData[$(this).attr('field')] = this.value;
                    });
                });
                return rows;
            },
            // 获取当前表格中所有行，包括隐藏字段
            getAllRows: function () {
                var rows = [];
                $('.subgrid_row', _instance.dom.$grid).each(function (index, row) {
                    var row = $(this);
                    var rowData = JSON.parse(JSON.stringify(row.data('rowData'))) || {};
                    row.find('[field]').each(function (i, input) {
                        rowData[$(this).attr('field')] = this.value;
                    });
                    rows.push(rowData);
                });
                return rows;
            },
            // 根据单元格dom对象(这里只有一个文本框input)获取当前行tr的jquery对象
            getRow: function (input) {
                return $(input).parent().parent() || {};
            },
            // 根据单元格dom对象(这里只有一个文本框input)获取当前行tr的json数据
            getRowData: function (input) {
                return $(input).parent().parent().data('rowData') || {};
            },
            // 根据单元格dom对象(这里只有一个文本框input)获取当前行tr的json数据field键的值
            getRowFieldValue: function (input, field) {
                return $(input).parent().parent().data('rowData')[field] || {};
            },
            // 全局输入校验
            isValid: function () {
                var isvalid = true;
                $('.subgrid_row', _instance.dom.$grid).each(function (index, row) {
                    if (!isvalid) { return false; }
                    $(this).find('[field]').each(function (i, input) {
                        if (!isvalid) { return false; }
                        var cfg = _instance.cols[$(this).attr('field')];
                        if (cfg && cfg.required && !this.value) {
                            $(this).parent().addClass('invalid');
                            isvalid = false; return false;
                        }
                        if (cfg && cfg.validator && $.isFunction(cfg.validator)) {
                            if (cfg.validator(this) === false) {
                                $(this).parent().addClass('invalid'); isvalid = false; return false;
                            }
                        }
                    });
                });
                return isvalid;
            }
        };
    };
})(jQuery);