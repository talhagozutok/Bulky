USE BULKY;

SELECT dbo.AspNetRoles.Id as UserId,
	dbo.AspNetUsers.UserName as UserName,
	dbo.AspNetRoles.Name as RoleName
FROM dbo.AspNetUsers
	JOIN dbo.AspNetUserRoles ON dbo.AspNetUserRoles.UserId = dbo.AspNetUsers.Id
	JOIN dbo.AspNetRoles ON dbo.AspNetRoles.Id = dbo.AspNetUserRoles.RoleId
WHERE UserName = 'admin@user.com'