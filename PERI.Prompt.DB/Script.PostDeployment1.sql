/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
--------------------------------------------------------------------------------------
insert into [Role](Name) values('Admin');
insert into [Role](Name) values('Blogger');
insert into [Role](Name) values('Reader');
--------------------------------------------------------------------------------------
insert into Gallery (Name, CreatedBy, DateCreated, ModifiedBy, DateModified) values('System', 'System', getdate(), 'System', getdate());
--------------------------------------------------------------------------------------
insert into BlogSortOrder values('Asynchronous by DateCreated');
insert into BlogSortOrder values('Desynchronous by DateCreated');
insert into BlogSortOrder values('Asynchronous by DatePublished');
insert into BlogSortOrder values('Desynchronous by DatePublished');
insert into BlogSortOrder values('Asynchronous by Title');
insert into BlogSortOrder values('Desynchronous by Title');
-------------------------------------------------------------------------------------
insert into Category(BlogSortOrderId, Name, CreatedBy, DateCreated, ModifiedBy, DateModified) values(4, 'Default', 'system', getdate(), 'system', getdate() );
-------------------------------------------------------------------------------------
insert into Template values('Default', null)
-------------------------------------------------------------------------------------
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Config', 'Max user','0', 'number', 1, 'true');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Config', 'Allow signup','0', 'number', 1, 'true');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Config', 'Default RoleId','3', 'number', 1, 'true');

insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Smtp', 'Smtp server', null,'text',  2, 'false');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Smtp', 'Smtp port',null,'number',  2, 'false');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Smtp', 'Smtp user', null,'email',  2, 'false');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Smtp', 'Smtp password', null,'password',  2, 'false');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Smtp', 'Smtp use ssl', null,'text',  2, 'false');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Smtp', 'Smtp display name', null,'text',  2, 'false');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Smtp', 'Smtp display email', null,'email',  2, 'false');

insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Recaptcha','Recaptcha public key', null,'text',  3, 'false');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Recaptcha','Recaptcha private key', null,'text',  3, 'false');

insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Site','Name', null,'text',  0, 'true');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Site','Tagline', null,'text',  0, 'true');
-------------------------------------------------------------------------------------
insert into [Menu](Name) values('Homepage');
insert into [Menu](Name) values('Header');
insert into [Menu](Name) values('Footer');
-------------------------------------------------------------------------------------
insert into [MenuItem](MenuId, Label, Url, [Order]) values(1, 'Home', '/', 0);