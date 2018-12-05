using Sourceportal.Domain.Models.API.Requests.WorkflowManagement;
using Sourceportal.Domain.Models.API.Responses.WorkflowManagement;

namespace SourcePortal.Services.WorkflowManagement
{
    public interface IWorkflowManagementService
    {
        StateEngineConditionsGetResponse StateEngineConditionsGet(int objectTypeId);
        StateEngineRuleGroupsGetResponse StateEngineConditionRuleGroupsGet(int ruleId);
        StateEngineTriggersGetResponse StateEngineTriggersGet(int objectTypeId);
        StateEngineRulesGetResponse StateEngineRulesGet(int objectTypeId);
        RuleDetailSetResponse StateEngineRuleDetailSet(RuleDetailSetRequest ruleDetailSetRequest);
        RuleDetailSetResponse StateEngineRuleDelete(RuleDetailSetRequest ruleDetailSetRequest);
        StateEngineRuleActionsGetResponse StateEngineConditionRuleActionsGet(int ruleId);
        StateEngineActionsGetResponse StateEngineActionsGet(int objectTypeId);
        RuleObjectTypesGetResponse RuleObjectTypesGet();
    }
}
