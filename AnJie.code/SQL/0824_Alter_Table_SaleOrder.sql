USE AnJieERP
GO	

ALTER TABLE dbo.SaleOrder ADD LockUserId  NVARCHAR(50)
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'�������ʺ�',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'SaleOrder', @level2type = N'COLUMN',
    @level2name = N'LockUserId';
GO

ALTER TABLE dbo.SaleOrder ADD LockUserName  NVARCHAR(50)
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'����������',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'SaleOrder', @level2type = N'COLUMN',
    @level2name = N'LockUserName';
GO


ALTER TABLE dbo.SaleOrder ADD UnLockTime  DATETIME
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'����ʱ��',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'SaleOrder', @level2type = N'COLUMN',
    @level2name = N'UnLockTime';
GO



--UPDATE SaleOrder SET LockUserId='',LockUserName='',UnLockTime=DATEADD(MINUTE,-1,GETDATE()) WHERE LockUserId IS NULL