using System.Web.Http;
using Sourceportal.Domain.Models.API.Requests.WorkflowManagement;
using Sourceportal.Domain.Models.API.Responses.WorkflowManagement;
using SourcePortal.Services.WorkflowManagement;

namespace Sourceportal.API.Controllers
{
    public class WorkflowManagementController : ApiController
    {
        private readonly IWorkflowManagementService _workflowManagementService;

        public WorkflowManagementController(IWorkflowManagementService workflowManagementService)
        {
            _workflowManagementService = workflowManagementService;
        }

        [Authorize]
        [HttpGet]
        [Route("api/workflow-management/stateEngineConditionsGet")]
        public StateEngineConditionsGetResponse StateEngineConditionsGet(int objectTypeId)
        {
            return _workflowManagementService.StateEngineConditionsGet(objectTypeId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/workflow-management/stateEngineRuleGroupsGet")]
        public StateEngineRuleGroupsGetResponse StateEngineRuleGroupsGet(int ruleId)
        {
            return _workflowManagementService.StateEngineConditionRuleGroupsGet(ruleId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/workflow-management/stateEngineTriggersGet")]
        public StateEngineTriggersGetResponse StateEngineTriggersGet(int objectTypeId)
        {
            return _workflowManagementService.StateEngineTriggersGet(objectTypeId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/workflow-management/stateEngineRulesGet")]
        public StateEngineRulesGetResponse StateEngineRulesGet(int objectTypeId)
        {
            return _workflowManagementService.StateEngineRulesGet(objectTypeId);
        }

        [Authorize]
        [HttpPost]
        [Route("api/workflow-management/ruleDetailSet")]
        public RuleDetailSetResponse StateEngineRuleDetailSet( RuleDetailSetRequest ruleDetailSetRequest)
        {
            return _workflowManagementService.StateEngineRuleDetailSet(ruleDetailSetRequest);
        }

        [Authorize]
        [HttpPost]
        [Route("api/workflow-management/ruleDelete")]
        public RuleDetailSetResponse StateEngineRuleDelete(RuleDetailSetRequest ruleDetailSetRequest)
        {
            return _workflowManagementService.StateEngineRuleDelete(ruleDetailSetRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/workflow-management/stateEngineRuleActionsGet")]
        public StateEngineRuleActionsGetResponse StateEngineRuleActionsGet(int ruleId)
        {
            return _workflowManagementService.StateEngineConditionRuleActionsGet(ruleId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/workflow-management/stateEngineActionsGet")]
        public StateEngineActionsGetResponse StateEngineRuleActionListGet(int objectTypeId)
        {
            return _workflowManagementService.StateEngineActionsGet(objectTypeId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/workflow-management/stateEngineObjectTypesGet")]
        public RuleObjectTypesGetResponse RuleObjectTypesGet()
        {
            return _workflowManagementService.RuleObjectTypesGet();
        }

    }
}