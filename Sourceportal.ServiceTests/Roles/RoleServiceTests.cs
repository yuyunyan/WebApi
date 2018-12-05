using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;
using NUnit.Framework;
using Rhino.Mocks;
using Sourceportal.DB.Enum;
using Sourceportal.DB.Roles;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.DB.Roles;
using SourcePortal.Services.Roles;
using ObjectType = Sourceportal.DB.Enum.ObjectType;


namespace Sourceportal.ServiceTests.Roles
{
    [TestFixture]
    public class RoleServiceTests
    {
        [Test]
        public void Should_Fields_And_Permssion_Be_Null_When_ObjectType_Is_Navigation()
        {
            var objectTypeId = (int)ObjectType.Navigation;
            var roleRepository = MockRepository.GenerateMock<IRoleRepository>();
            roleRepository.Stub(x => x.GetNavigationLinksForType()).IgnoreArguments().Return(new List<DbNavigationLink>());
            roleRepository.Stub(x => x.GetNavigationLinksForRole(0)).IgnoreArguments().Return(new List<DbNavigationLink>());
            roleRepository.Stub(x => x.GetRoleStaticData(0)).Return(new RoleDetailsResponse {ObjectTypeId = objectTypeId });

            var rolesService = new RoleService(roleRepository);
            
            var results = rolesService.GetRoleDetails(0);
            Assert.That(results.Data.Fields, Is.Null);
            Assert.That(results.Data.Permissions, Is.Null);
        }

        [Test]
        public void Should_Return_Fields_And_Permission_When_ObjectType_Is_Not_Navigation()
        {
            var roleRepository = MockRepository.GenerateMock<IRoleRepository>();
            roleRepository.Stub(x => x.GetFieldsForType(ObjectType.Accounts)).IgnoreArguments().Return(new List<DbField>());
            roleRepository.Stub(x => x.GetFieldsForRole(1)).IgnoreArguments().Return(new List<DbField>());
            roleRepository.Stub(x => x.GetPermissionsForType(ObjectType.Accounts)).IgnoreArguments().Return(new List<DbPermission>());
            roleRepository.Stub(x => x.GetPermissionsForRole(1)).IgnoreArguments().Return(new List<DbPermission>());
            roleRepository.Stub(x => x.GetRoleStaticData(0)).Return(new RoleDetailsResponse());

            var rolesService = new RoleService(roleRepository);
            var results = rolesService.GetRoleDetails(0);
            Assert.That(results.Data.Fields, Is.Not.Null);
            Assert.That(results.Data.Permissions, Is.Not.Null);
            Assert.That(results.Data.NavigationLinks, Is.Null);
        }

        [Test]
        public void Should_Populate_Navigation_Links()
        {
            var roleRepository = MockRepository.GenerateMock<IRoleRepository>();
            var objectTypeId = (int)ObjectType.Navigation;
            var roleId = 123;
            roleRepository.Expect(x => x.GetNavigationLinksForType()).Return(
                new List<DbNavigationLink> {
                    new DbNavigationLink{NavID = 1, NavName = "aa"},
                    new DbNavigationLink{NavID = 2, NavName = "bb"}
                });
            roleRepository.Stub(x => x.GetNavigationLinksForRole(0)).IgnoreArguments().Return(new List<DbNavigationLink>());
            roleRepository.Stub(x => x.GetRoleStaticData(roleId)).Return(new RoleDetailsResponse {ObjectTypeId = objectTypeId });

            var rolesService = new RoleService(roleRepository);
            
            var results = rolesService.GetRoleDetails(roleId);

            roleRepository.VerifyAllExpectations();
            Assert.That(results.Data.NavigationLinks.Count, Is.EqualTo(2));
            Assert.That(results.Data.NavigationLinks[0].NavName, Is.EqualTo("aa"));
            Assert.That(results.Data.NavigationLinks[0].NavId, Is.EqualTo(1));
            Assert.That(results.Data.NavigationLinks[1].NavName, Is.EqualTo("bb"));
            Assert.That(results.Data.NavigationLinks[1].NavId, Is.EqualTo(2));
        }

        [Test]
        public void Should_Populate_Selected_For_Role_When_Role_Has_Navigation_Links()
        {
            var roleRepository = MockRepository.GenerateMock<IRoleRepository>();
            var objectTypeId = (int)ObjectType.Navigation;
            var roleId = 123;
            roleRepository.Expect(x => x.GetNavigationLinksForType()).Return(
                new List<DbNavigationLink> {
                    new DbNavigationLink{NavID = 1, NavName = "aa"},
                    new DbNavigationLink{NavID = 2, NavName = "bb"}
                });

            roleRepository.Expect(x => x.GetNavigationLinksForRole(roleId)).Return(
                new List<DbNavigationLink> {
                    new DbNavigationLink{NavID = 1, NavName = "aa"}
                });
            roleRepository.Stub(x => x.GetRoleStaticData(roleId)).Return(new RoleDetailsResponse { ObjectTypeId = objectTypeId });

            var rolesService = new RoleService(roleRepository);

            var results = rolesService.GetRoleDetails(roleId);

            roleRepository.VerifyAllExpectations();
            Assert.That(results.Data.NavigationLinks[0].SelectedForRole, Is.True);
            Assert.That(results.Data.NavigationLinks[1].SelectedForRole, Is.False);
        }

        [Test]
        public void Should_Group_Navigation_Links()
        {
            var objectTypeId = (int)ObjectType.Navigation;
            var roleRepository = MockRepository.GenerateMock<IRoleRepository>();
            var roleId = 123;
            var dbNavigationLinks = new List<DbNavigationLink> {
                new DbNavigationLink{NavID = 1, NavName = "aa"},
                new DbNavigationLink{NavID = 2, NavName = "bb"},
                new DbNavigationLink{NavID = 3, NavName = "bb", ParentNavID = 1},
                new DbNavigationLink{NavID = 4, NavName = "bb", ParentNavID = 1},
                new DbNavigationLink{NavID = 5, NavName = "bb", ParentNavID = 2},
                new DbNavigationLink{NavID = 6, NavName = "bb"},
            };
            var fullLinkList = new List<DbNavigationLink> {
                new DbNavigationLink{NavID = 1, NavName = "aa"},
                new DbNavigationLink{NavID = 2, NavName = "bb"},
                new DbNavigationLink{NavID = 3, NavName = "bb", ParentNavID = 1},
                new DbNavigationLink{NavID = 4, NavName = "bb", ParentNavID = 1},
                new DbNavigationLink{NavID = 5, NavName = "bb", ParentNavID = 2},
                new DbNavigationLink{NavID = 6, NavName = "bb"},
                new DbNavigationLink{NavID = 7, NavName = "bb", ParentNavID = 1},
            };
            
            roleRepository.Stub(x => x.GetNavigationLinksForType()).Return(fullLinkList);
            roleRepository.Stub(x => x.GetNavigationLinksForRole(roleId)).Return(dbNavigationLinks);
            roleRepository.Stub(x => x.GetRoleStaticData(roleId)).Return(new RoleDetailsResponse {ObjectTypeId = objectTypeId });

            var rolesService = new RoleService(roleRepository);

            var results = rolesService.GetRoleDetails(roleId);

            Assert.That(results.Data.NavigationLinks.Count, Is.EqualTo(3));
            Assert.That(results.Data.NavigationLinks[0].ChildNodes.Count, Is.EqualTo(3));
            Assert.That(results.Data.NavigationLinks[0].ChildNodes[0].NavId, Is.EqualTo(3));
            Assert.That(results.Data.NavigationLinks[0].ChildNodes[1].NavId, Is.EqualTo(4));
            Assert.That(results.Data.NavigationLinks[0].ChildNodes[2].NavId, Is.EqualTo(7));
            Assert.That(results.Data.NavigationLinks[1].ChildNodes.Count, Is.EqualTo(1));
            Assert.That(results.Data.NavigationLinks[1].ChildNodes[0].NavId, Is.EqualTo(5));
            Assert.That(results.Data.NavigationLinks[2].ChildNodes, Is.Null);
            
        }

        [Test]
        public void Should_Populate_Fields_When_Role_Is_Not_Navigation()
        {
            var roleRepository = MockRepository.GenerateMock<IRoleRepository>();
            var objectTypeId = (int)ObjectType.Accounts;

            var roleId = 123;
            
            roleRepository.Expect(x => x.GetFieldsForType(ObjectType.Accounts)).Return(
                new List<DbField> {
                    new DbField{ FieldId = 1, FieldName = "aa", PermissionId = (int)PermissionType.CanEditAccountField },
                    new DbField{ FieldId = 2, FieldName = "bb", PermissionId = (int)PermissionType.CanViewAccountField  }
                });
            roleRepository.Stub(x => x.GetFieldsForRole(1)).IgnoreArguments().Return(new List<DbField>());
            roleRepository.Stub(x => x.GetPermissionsForRole(1)).IgnoreArguments().Return(new List<DbPermission>());
            roleRepository.Stub(x => x.GetPermissionsForType(ObjectType.Accounts)).IgnoreArguments().Return(new List<DbPermission>());
            roleRepository.Stub(x => x.GetRoleStaticData(roleId)).Return(new RoleDetailsResponse {ObjectTypeId = objectTypeId });

            var rolesService = new RoleService(roleRepository);

            var results = rolesService.GetRoleDetails(roleId);

            roleRepository.VerifyAllExpectations();
            Assert.That(results.Data.Fields.Count, Is.EqualTo(2));
            Assert.That(results.Data.Fields[0].FieldName, Is.EqualTo("aa"));
            Assert.That(results.Data.Fields[0].FieldID, Is.EqualTo(1));
            Assert.That(results.Data.Fields[1].FieldName, Is.EqualTo("bb"));
            Assert.That(results.Data.Fields[1].IsEditable, Is.False);
        }

        [Test]
        public void Should_Set_Select_For_Roles_In_Fields()
        {
            var roleRepository = MockRepository.GenerateMock<IRoleRepository>();
            var objectTypeId = (int)ObjectType.Accounts;

            var roleId = 123;
            roleRepository.Stub(x => x.GetFieldsForType(ObjectType.Accounts)).Return(
                new List<DbField> {
                    new DbField{ FieldId = 1, FieldName = "aa" },
                    new DbField{ FieldId = 2, FieldName = "bb" },
                    new DbField{ FieldId = 3, FieldName = "bb" }
                });

            roleRepository.Stub(x => x.GetRoleStaticData(roleId)).Return(new RoleDetailsResponse {ObjectTypeId = objectTypeId });
            roleRepository.Expect(x => x.GetFieldsForRole(roleId)).Return(
                new List<DbField> {
                    new DbField{ FieldId = 1, FieldName = "aa", PermissionId = (int)PermissionType.CanEditAccountField },
                    new DbField{ FieldId = 2, FieldName = "bb", PermissionId = (int)PermissionType.CanViewAccountField  }
                });

            roleRepository.Stub(x => x.GetPermissionsForRole(1)).IgnoreArguments().Return(new List<DbPermission>());
            roleRepository.Stub(x => x.GetPermissionsForType(ObjectType.Accounts)).IgnoreArguments().Return(new List<DbPermission>());

            var rolesService = new RoleService(roleRepository);
            
            var results = rolesService.GetRoleDetails(roleId);

            roleRepository.VerifyAllExpectations();

            Assert.That(results.Data.Fields[0].SelectedForRole, Is.True);
            Assert.That(results.Data.Fields[1].SelectedForRole, Is.True);
            Assert.That(results.Data.Fields[2].SelectedForRole, Is.False);
            
        }

        [Test]
        public void Should_Set_Is_Editable_For_Fields()
        {
            var roleRepository = MockRepository.GenerateMock<IRoleRepository>();
            var roleId = 123;
            var objectTypeId = (int)ObjectType.Accounts;
            roleRepository.Stub(x => x.GetFieldsForType(ObjectType.Accounts)).Return(
                new List<DbField> {
                    new DbField{ FieldId = 1, FieldName = "aa" },
                    new DbField{ FieldId = 2, FieldName = "bb" },
                    new DbField{ FieldId = 3, FieldName = "bb" }
                });

            roleRepository.Expect(x => x.GetFieldsForRole(roleId)).Return(
                new List<DbField> {
                    new DbField{ FieldId = 1, FieldName = "aa", PermissionId = (int)PermissionType.CanEditAccountField},
                    new DbField{ FieldId = 2, FieldName = "bb", PermissionId = (int)PermissionType.CanViewAccountField }
                });
            roleRepository.Stub(x => x.GetRoleStaticData(roleId)).Return(new RoleDetailsResponse {ObjectTypeId = objectTypeId });
            roleRepository.Stub(x => x.GetPermissionsForRole(1)).IgnoreArguments().Return(new List<DbPermission>());
            roleRepository.Stub(x => x.GetPermissionsForType(ObjectType.Accounts)).IgnoreArguments().Return(new List<DbPermission>());
            var rolesService = new RoleService(roleRepository);
            
            var results = rolesService.GetRoleDetails(roleId);

            roleRepository.VerifyAllExpectations();

            Assert.That(results.Data.Fields[0].IsEditable, Is.True);
            Assert.That(results.Data.Fields[1].IsEditable, Is.False);
            Assert.That(results.Data.Fields[2].IsEditable, Is.False);
        }

        [Test]
        public void Should_Populate_Permissions_When_ObjectTypeId_Is_Not_Navigation()
        {
            var roleRepository = MockRepository.GenerateMock<IRoleRepository>();
            var objectTypeId = (int)ObjectType.Accounts;
            var roleId = 123;

            roleRepository.Expect(x => x.GetPermissionsForType(ObjectType.Accounts)).Return(
                new List<DbPermission> {
                    new DbPermission{  PermName = "Merge Accounts", Description = "I can Merge", PermissionID = 1},
                    new DbPermission{  PermName = "Change Credit", Description = "I can Credit", PermissionID = 2 }
                });
            roleRepository.Stub(x => x.GetFieldsForRole(1)).IgnoreArguments().Return(new List<DbField>());
            roleRepository.Stub(x => x.GetFieldsForType(ObjectType.Accounts)).IgnoreArguments().Return(new List<DbField>());
            roleRepository.Stub(x => x.GetPermissionsForRole(1)).IgnoreArguments().Return(new List<DbPermission>());
            roleRepository.Stub(x => x.GetRoleStaticData(roleId)).Return(new RoleDetailsResponse {ObjectTypeId = objectTypeId });
            var rolesService = new RoleService(roleRepository);

            var results = rolesService.GetRoleDetails(roleId);

            roleRepository.VerifyAllExpectations();
            Assert.That(results.Data.Permissions.Count, Is.EqualTo(2));

            Assert.That(results.Data.Permissions[0].PermissionID, Is.EqualTo(1));
            Assert.That(results.Data.Permissions[0].PermName, Is.EqualTo("Merge Accounts"));
            Assert.That(results.Data.Permissions[0].Description, Is.EqualTo("I can Merge"));
            Assert.That(results.Data.Permissions[1].PermissionID, Is.EqualTo(2));
            Assert.That(results.Data.Permissions[1].PermName, Is.EqualTo("Change Credit"));
            Assert.That(results.Data.Permissions[1].Description, Is.EqualTo("I can Credit"));
        }

        [Test]
        public void Should_Populate_Set_For_Role_In_Permissions()
        {
            var roleRepository = MockRepository.GenerateMock<IRoleRepository>();
            var objectTypeId = (int)ObjectType.Accounts;
            var roleId = 123;
            roleRepository.Stub(x => x.GetPermissionsForType(ObjectType.Accounts)).Return(
                new List<DbPermission> {
                    new DbPermission{  PermName = "Merge Accounts", Description = "I can Merge", PermissionID = 1},
                    new DbPermission{  PermName = "Change Status", Description = "I can do Status", PermissionID = 2 },
                    new DbPermission{  PermName = "Change Credit", Description = "I can Credit", PermissionID = 3 }
                });

            roleRepository.Expect(x => x.GetPermissionsForRole(roleId)).Return(
                new List<DbPermission> {
                    new DbPermission{  PermName = "Merge Accounts", Description = "I can Merge", PermissionID =1},
                    new DbPermission{  PermName = "Change Credit", Description = "I can Credit", PermissionID =3 }
                });roleRepository.Stub(x => x.GetRoleStaticData(roleId)).Return(new RoleDetailsResponse { ObjectTypeId = objectTypeId });
            roleRepository.Stub(x => x.GetFieldsForRole(1)).IgnoreArguments().Return(new List<DbField>());
            roleRepository.Stub(x => x.GetFieldsForType(ObjectType.Accounts)).IgnoreArguments().Return(new List<DbField>());

            var rolesService = new RoleService(roleRepository);

            var results = rolesService.GetRoleDetails(roleId);

            roleRepository.VerifyAllExpectations();
           
            Assert.That(results.Data.Permissions[0].SelectedForRole, Is.True);
            Assert.That(results.Data.Permissions[1].SelectedForRole, Is.False);
            Assert.That(results.Data.Permissions[2].SelectedForRole, Is.True);
        }

        [Test]
        public void Should_Get_All_Navigation_Links()
        {
            var roleRepository = MockRepository.GenerateMock<IRoleRepository>();
            var roleId = 123;
            roleRepository.Expect(x => x.GetNavigationLinksForType()).Return(
                new List<DbNavigationLink> {
                    new DbNavigationLink{NavID = 1, NavName = "aa"},
                    new DbNavigationLink{NavID = 2, NavName = "bb"}
                });
            roleRepository.Stub(x => x.GetNavigationLinksForRole(0)).IgnoreArguments().Return(
                new List<DbNavigationLink> { new DbNavigationLink { NavID = 2, NavName = "bb" } });
            
            var rolesService = new RoleService(roleRepository);

            var results = rolesService.GetDataToCreateNewRole(ObjectType.Navigation);

            roleRepository.VerifyAllExpectations();
            Assert.That(results.Data.NavigationLinks.Count, Is.EqualTo(2));
            Assert.That(results.Data.NavigationLinks[0].NavName, Is.EqualTo("aa"));
            Assert.That(results.Data.NavigationLinks[0].NavId, Is.EqualTo(1));
            Assert.That(results.Data.NavigationLinks[1].NavName, Is.EqualTo("bb"));
            Assert.That(results.Data.NavigationLinks[1].NavId, Is.EqualTo(2));
            Assert.False(results.Data.NavigationLinks.Any(x => x.SelectedForRole));
        }

        [Test]
        public void Should_Get_All_Fields()
        {
            var roleRepository = MockRepository.GenerateMock<IRoleRepository>();

            var roleId = 123;

            roleRepository.Expect(x => x.GetFieldsForType(ObjectType.Accounts)).Return(
                new List<DbField> {
                    new DbField{ FieldId = 1, FieldName= "aa", PermissionId = (int)PermissionType.CanEditAccountField },
                    new DbField{ FieldId = 2, FieldName = "bb", PermissionId = (int)PermissionType.CanViewAccountField  }
                });
            roleRepository.Stub(x => x.GetFieldsForRole(1)).IgnoreArguments().Return(
                new List<DbField> {
                    new DbField{ FieldId = 1, FieldName = "aa", PermissionId = (int)PermissionType.CanEditAccountField}
                });

            roleRepository.Stub(x => x.GetPermissionsForRole(1)).IgnoreArguments().Return(new List<DbPermission>());
            roleRepository.Stub(x => x.GetPermissionsForType(ObjectType.Accounts)).IgnoreArguments().Return(new List<DbPermission>());
            
            var rolesService = new RoleService(roleRepository);

            var results = rolesService.GetDataToCreateNewRole(ObjectType.Accounts);

            roleRepository.VerifyAllExpectations();
            Assert.That(results.Data.Fields.Count, Is.EqualTo(2));
            Assert.That(results.Data.Fields[0].FieldName, Is.EqualTo("aa"));
            Assert.That(results.Data.Fields[0].FieldID, Is.EqualTo(1));
            Assert.That(results.Data.Fields[1].FieldName, Is.EqualTo("bb"));
            Assert.That(results.Data.Fields[1].FieldID, Is.EqualTo(2));
            Assert.False(results.Data.Fields.Any(x => x.SelectedForRole));
        }

        [Test]
        public void Should_Get_All_Permissions()
        {
            var roleRepository = MockRepository.GenerateMock<IRoleRepository>();

            roleRepository.Stub(x => x.GetPermissionsForType(ObjectType.Accounts)).Return(
                new List<DbPermission> {
                    new DbPermission{  PermName = "Merge Accounts", Description = "I can Merge", PermissionID = 1},
                    new DbPermission{  PermName = "Change Status", Description = "I can do Status", PermissionID = 2 },
                    new DbPermission{  PermName = "Change Credit", Description = "I can Credit", PermissionID = 3 }
                });

            roleRepository.Stub(x => x.GetFieldsForRole(1)).IgnoreArguments().Return(new List<DbField>());
            roleRepository.Stub(x => x.GetFieldsForType(ObjectType.Accounts)).IgnoreArguments().Return(new List<DbField>());

            var rolesService = new RoleService(roleRepository);

            var results = rolesService.GetDataToCreateNewRole(ObjectType.Accounts);

            roleRepository.VerifyAllExpectations();

            Assert.That(results.Data.Permissions.Count, Is.EqualTo(3));
            Assert.That(results.Data.Permissions[0].PermissionID, Is.EqualTo(1));
            Assert.That(results.Data.Permissions[1].PermissionID, Is.EqualTo(2));
            Assert.That(results.Data.Permissions[2].PermissionID, Is.EqualTo(3));
            Assert.False(results.Data.Permissions.Any(x=> x.SelectedForRole));
        }
    }
}
