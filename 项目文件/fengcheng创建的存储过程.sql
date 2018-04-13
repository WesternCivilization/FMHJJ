USE [FMHJJ]
GO
/****** Object:  StoredProcedure [dbo].[GetMenus]    Script Date: 04/10/2018 21:08:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Proc [dbo].[GetMenus](@UserID int, @SuperName varchar(max), @UserType int)
as
 begin
	declare @UserName varchar(50)
	select @UserName=UserName from Base_UserInfo where ID=@UserID
	declare @Temp_Variable varchar(max)
	declare @temp table (Item varchar(max))
	while(LEN(@SuperName) > 0)
	begin
		if(CHARINDEX(',', @SuperName) = 0)
		begin
			set @Temp_Variable = @SuperName
			set @SuperName = ''
		end
		else
		begin
			set @Temp_Variable = LEFT(@SuperName, CHARINDEX(',', @SuperName)-1)
			set @SuperName = RIGHT(@SuperName, LEN(@SuperName) - LEN(@Temp_Variable) -1)
		end	
		insert @temp values(@Temp_Variable)
	end
		
 	if exists(select 1 from @temp where Item=@UserName)
	begin
		SELECT DISTINCT ID,PID,FunctionName,Icon,Url FROM Dict_FunctionMenu 
	end
 	else 
 	begin
 		CREATE TABLE #MENU (FunctionID INT NULL)
 		if (@UserType=2)
		begin
			insert into #MENU select ID from Dict_FunctionMenu where UserType in (0,2) 			
		end
 		else
 		begin 			
			insert into #MENU select FunctionID from Base_UserInfo_Grant where IsAllow=1 and UserID=@UserID
			--insert into #MENU select ID from Dict_FunctionMenu where UserType=0							
 		end 
 		
 		;WITH 
			LOCS(ID,PID,FunctionName,Icon,Url)
			AS
			(
			SELECT ID,PID,FunctionName,Icon,Url FROM Dict_FunctionMenu WHERE ID in (select FunctionID from #MENU)
			UNION ALL
			SELECT A.ID,A.PID,A.FunctionName,A.Icon,A.Url FROM Dict_FunctionMenu A JOIN LOCS B ON 
			B.PID=A.ID 
			--A.pid = B.id
			)

			select DISTINCT ID,PID,FunctionName,Icon,Url from LOCS			
	 end
 end

GO

CREATE Proc [dbo].[SelectUserFunction](@UserID int)
as
 begin
	select distinct a.PID, a.ID, a.FunctionName, b.IsAllow
	from Dict_FunctionMenu a left join Base_UserInfo_Grant b on a.ID=b.FunctionID and b.UserID=@UserID
	where a.UserType in (0, 1, 2)
	order by a.PID, a.ID
 end

GO

CREATE Proc [dbo].[GetFinalist](@BidID int)
as
 begin try
	--begin transaction 
	declare @tempBid table (ID int IDENTITY(1,1), UserID int NULL, BidAmount numeric(11,2) NULL, BidPrice numeric(11,2) NULL, BidTime datetime NULL)	
	insert @tempBid 
	select a.UserID,a.BidAmount,a.BidPrice,a.BidTime from Flw_BiddingInfo a with(nolock) inner join Base_UserInfo b on a.UserID=b.ID
	where a.BidID=@BidID and a.Checked=1
	order by a.BidPrice desc, b.UserLvl asc, a.BidAmount desc, a.BidTime asc
	
	declare @tempFinal table (ID int IDENTITY(1,1), UserID int NULL, BidAmount numeric(11,2) NULL, BidPrice numeric(11,2) NULL, BidTime datetime NULL, DealAmount numeric(11,2) NULL, DealPrice numeric(11,2) NULL)			
	declare @TotalAmount numeric(11,2)
	select @TotalAmount=EstimateAmount from Data_BidManage where ID=@BidID
			
	declare @BidAmount numeric(11,2)
	declare @TempBidID int
	declare @TempFinalID int	
	WHILE @TotalAmount > 0 and EXISTS(SELECT UserID FROM @tempBid)
	BEGIN 		
		SET ROWCOUNT 1
		select @BidAmount=BidAmount, @TempBidID=ID from @tempBid	
		insert @tempFinal select UserID,BidAmount,BidPrice,BidTime,BidAmount as DealAmount, BidPrice as DealPrice from @tempBid	
		select @TempFinalID=@@identity
		if(@TotalAmount <= @BidAmount)
		begin			
			update @tempFinal set DealAmount=@TotalAmount where ID=@TempFinalID
			set @TotalAmount = 0
		end
		else
		begin
			set @TotalAmount=@TotalAmount-@BidAmount
		end	
		SET ROWCOUNT 0
		--删除临时表中的使用完的数据
		DELETE from @tempBid where ID=@TempBidID
	END 
	
	--余量均分
	IF @TotalAmount > 0
	BEGIN
		declare @FinalCount int
		declare @AvgAmount numeric(11,2)
		select @FinalCount=COUNT(*) from @tempFinal
		select @AvgAmount=cast(@TotalAmount/@FinalCount as numeric(11,2)) 
		update @tempFinal set DealAmount=DealAmount+@AvgAmount		
	END		
	
	--commit transaction

	select * from @tempFinal
	
 end try   
 begin catch  
	--rollback transaction
 end catch



