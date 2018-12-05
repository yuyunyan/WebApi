CREATE VIEW [dbo].[vwUserAccess]
AS
SELECT ur.UserID, r.ObjectTypeID, r.RoleID, ur.FilterObjectTypeID, ur.FilterTypeID, ur.FilterObjectID
FROM mapUserRoles ur
INNER JOIN Roles r ON ur.RoleID = r.RoleID
WHERE r.IsDeleted = 0 AND ur.IsDeleted = 0
