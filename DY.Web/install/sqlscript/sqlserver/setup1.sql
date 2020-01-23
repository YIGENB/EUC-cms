declare @tname varchar(8000)
set @tname=''
select @tname=@tname + Name + ',' from sysobjects where xtype='U'
select @tname='drop table ' + left(@tname,len(@tname)-1)
exec(@tname)
go
declare @tname varchar(8000)
set @tname=''
select @tname=@tname + Name + ',' from sysobjects where xtype='P'
select @tname='drop Procedure ' + left(@tname,len(@tname)-1)
exec(@tname)