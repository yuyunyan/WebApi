using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sourceportal.DB.Enum;
using Sourceportal.DB.WorkflowManagement;
using Sourceportal.Domain.Models.API.Requests.WorkflowManagement;
using Sourceportal.Domain.Models.API.Responses.WorkflowManagement;
using Sourceportal.Domain.Models.DB.WorkflowManagement;

namespace SourcePortal.Services.WorkflowManagement
{
    public class WorkflowManagementService : IWorkflowManagementService
    {
        private readonly IWorkflowManagementRepository _workflowManagementRepository;
        
        public WorkflowManagementService(IWorkflowManagementRepository workflowManagementRepository)
        {
            _workflowManagementRepository = workflowManagementRepository;
        }

        public StateEngineConditionsGetResponse StateEngineConditionsGet(int objectTypeId)
        {
            var conditionDbs = _workflowManagementRepository.StateEngineConditionsGet(objectTypeId);
            var conditions = new List<StateEngineCondition>();
            foreach (var conditionDb in conditionDbs)
            {
                var condition = new StateEngineCondition
                {
                    ConditionID = conditionDb.ConditionID,
                    ConditionName = conditionDb.ConditionName,
                    ObjectTypeID = conditionDb.ObjectTypeID,
                    ComparisonType = conditionDb.ComparisonType,
                    DynamicValues = new List<StateEngineDynamicValue>()
                };

                var dynamicValues = !string.IsNullOrEmpty(conditionDb.DynamicValues)
                    ? JsonConvert.DeserializeObject<List<ConditionDynamicValueDb>>(conditionDb.DynamicValues)
                    : new List<ConditionDynamicValueDb>();
                if (dynamicValues.Count > 0)
                {
                    foreach (var dynamicValue in dynamicValues)
                    {
                        var dv = new StateEngineDynamicValue
                        {
                            ValueID = dynamicValue.ValueID,
                            ValueName = dynamicValue.ValueName
                        };
                        condition.DynamicValues.Add(dv);
                    }
                }
                conditions.Add(condition);
            }
            return new StateEngineConditionsGetResponse
            {
                StateEngineConditions = conditions
            };
        }

        public StateEngineRuleGroupsGetResponse StateEngineConditionRuleGroupsGet(int ruleId)
        {
            var ruleGroupDbs = _workflowManagementRepository.StateEngineRuleGroupsGet(ruleId);
            return new StateEngineRuleGroupsGetResponse
            {
                RootRuleGroup = BuildRuleGroupTree(ruleGroupDbs)
            };
        }

        public StateEngineTriggersGetResponse StateEngineTriggersGet(int objectTypeId)
        {
            var triggerDbs = _workflowManagementRepository.StateEngineTriggersGet(objectTypeId);
            var triggers = triggerDbs.Select(triggerDb => new StateEngineTriggerResponse
                {
                    ObjectTypeID = triggerDb.ObjectTypeID,
                    TriggerDescription = triggerDb.TriggerDescription,
                    TriggerID = triggerDb.TriggerID,
                    TriggerName = triggerDb.TriggerName
                })
                .ToList();
            return new StateEngineTriggersGetResponse
            {
                StateEngineTriggers = triggers
            };
        }

        public StateEngineRulesGetResponse StateEngineRulesGet(int objectTypeId)
        {
            var ruleDbs = _workflowManagementRepository.StateEngineRulesGet(objectTypeId);
            var rules = ruleDbs.Select(ruleDb => new StateEngineRuleResponse
                {
                    ObjectTypeID = ruleDb.ObjectTypeID,
                    TriggerID = ruleDb.TriggerID,
                    RuleID = ruleDb.RuleID,
                    RuleName = ruleDb.RuleName,
                    RuleOrder = ruleDb.RuleOrder
                })
                .ToList();
            return new StateEngineRulesGetResponse
            {
                StateEngineRuleList = rules
            };
        }

        public RuleDetailSetResponse StateEngineRuleDelete(RuleDetailSetRequest ruleDetailSetRequest)
        {
            var ruleId = _workflowManagementRepository.StateEngineRuleDetailSet(ruleDetailSetRequest);
            return new RuleDetailSetResponse
            {
                RuleID = ruleId,
                IsDeleted = 1
            };
        }

        public RuleDetailSetResponse StateEngineRuleDetailSet(RuleDetailSetRequest ruleDetailSetRequest)
        {
            var ruleId = _workflowManagementRepository.StateEngineRuleDetailSet(ruleDetailSetRequest);
            foreach (var ruleGroupRequest in ruleDetailSetRequest.Groups)
            {
                RuleGroupsSet(ruleGroupRequest, null, ruleId);
            }
            var rowCount = RuleActionsSet(ruleDetailSetRequest.Actions, ruleId);
            return new RuleDetailSetResponse
            {
                RuleID = ruleId,
                IsDeleted = ruleDetailSetRequest.IsDeleted,
                ObjectTypeID = ruleDetailSetRequest.ObjectTypeID ?? 0,
                TriggerID = ruleDetailSetRequest.TriggerID ?? 0,
                RuleName = ruleDetailSetRequest.RuleName,
                RuleOrder = ruleDetailSetRequest.RuleOrder ?? 0,
                ActionRowCount = rowCount
            };
        }

        public StateEngineRuleActionsGetResponse StateEngineConditionRuleActionsGet(int ruleId)
        {
            var ruleActions = new List<StateEngineRuleAction>();
            var ruleActionDbs = _workflowManagementRepository.StateEngineRuleActionsGet(ruleId);
            foreach (var ruleActionDb in ruleActionDbs)
            {
                var ruleAction = new StateEngineRuleAction
                {
                    ObjectTypeID = ruleActionDb.ObjectTypeID,
                    RuleID = ruleActionDb.RuleID,
                    ValueName = ruleActionDb.ValueName,
                    ValueID = ruleActionDb.ValueID,
                    StaticValue = ruleActionDb.StaticValue,
                    ActionID = ruleActionDb.ActionID,
                    ActionName = ruleActionDb.ActionName,
                    RuleActionID = ruleActionDb.RuleActionID
                };
                ruleActions.Add(ruleAction);
            }

            return new StateEngineRuleActionsGetResponse
            {
                RuleActions = ruleActions
            };
        }

        public StateEngineActionsGetResponse StateEngineActionsGet(int objectTypeId)
        {
            var actionDbs = _workflowManagementRepository.StateEngineActionListGet(objectTypeId);
            var actionList = new List<ActionResponse>();
            foreach (var actionDb in actionDbs)
            {
                var action = new ActionResponse
                {
                    ActionID = actionDb.ActionID,
                    ActionName = actionDb.ActionName,
                    ObjectTypeID = actionDb.ObjectTypeID,
                    DynamicValues = new List<StateEngineDynamicValue>()
                };

                var dynamicValues = !string.IsNullOrEmpty(actionDb.DynamicValues)
                    ? JsonConvert.DeserializeObject<List<ActionDynamicValueDb>>(actionDb.DynamicValues)
                    : new List<ActionDynamicValueDb>();
                if (dynamicValues.Count > 0)
                {
                    foreach (var dynamicValue in dynamicValues)
                    {
                        var dv = new StateEngineDynamicValue
                        {
                            ValueID = dynamicValue.ValueID,
                            ValueName = dynamicValue.ValueName
                        };
                        action.DynamicValues.Add(dv);
                    }
                }
                actionList.Add(action);
            }
            return new StateEngineActionsGetResponse
            {
                ActionList = actionList
            };
        }

        public RuleObjectTypesGetResponse RuleObjectTypesGet()
        {
            var RuleObjectTypeList = new List<RuleObjectType>
            {
                new RuleObjectType
                {
                    Name = "Quote",
                    ObjectTypeID = (int) ObjectType.Quote
                },
                new RuleObjectType
                {
                    Name = "Sales Order",
                    ObjectTypeID = (int) ObjectType.Salesorder
                },
                new RuleObjectType
                {
                    Name = "Purchase Order",
                    ObjectTypeID = (int) ObjectType.Purchaseorder
                },
                new RuleObjectType
                {
                    Name = "Inspection",
                    ObjectTypeID = (int) ObjectType.Inspection
                }
            };
            return new RuleObjectTypesGetResponse
            {
                RuleObjectTypes = RuleObjectTypeList
            };
        }

        private int RuleActionsSet(List<RuleActionRequest> ruleActionRequests, int ruleId)
        {
            var rowCount = _workflowManagementRepository.StateEngineRuleActionsSet(ruleActionRequests, ruleId);
            return rowCount;
        }

        private void RuleGroupsSet(RuleGroupRequest ruleGroupRequest, int? parentGroupId, int ruleId)
        {
            var ruleGroupId = _workflowManagementRepository.StateEngineRuleGroupSet(ruleGroupRequest, parentGroupId, ruleId);
            foreach (var ruleGroup in ruleGroupRequest.Groups)
            {
                RuleGroupsSet(ruleGroup, ruleGroupId, ruleId);
            }
        }

        public StateEngineRuleGroup BuildRuleGroupTree(IList<RuleGroupDb> ruleGroupDbs)
        {
            var ruleGroups = ruleGroupDbs.GroupBy(x => x.ParentGroupID);
            var rootRuleGroupDb = ruleGroupDbs.FirstOrDefault(x => x.ParentGroupID == 0);
            var dict = ruleGroups.Where(x => x.Key != 0).ToDictionary(g => g.Key, g => g.ToList());
            var rootRuleGroup = AppendColumn(rootRuleGroupDb, dict);
            return rootRuleGroup;
        }

        public StateEngineRuleGroup AppendColumn(RuleGroupDb node, IDictionary<int, List<RuleGroupDb>> dict)
        {
            var ruleGroupColumn = new StateEngineRuleGroup
            {
                Columns = new List<StateEngineRuleGroupColumn>(),
                IsAll = node.IsAll,
                ObjectTypeID = node.ObjectTypeID,
                ParentGroupID = (node.ParentGroupID == 0) ? null: (int?) node.ParentGroupID, 
                RuleGroupID = node.RuleGroupID,
                RuleID = node.RuleID
            };
            if (dict.ContainsKey(node.RuleGroupID))
            {
                var childRuleGroupDbs = dict[node.RuleGroupID];
                foreach (var chileRuleGroupDb in childRuleGroupDbs)
                {
                    var ruleGroup = AppendColumn(chileRuleGroupDb, dict);
                    ruleGroupColumn.Columns.Add(ruleGroup);
                }
            }
            var nodeConditions = JsonConvert.DeserializeObject<List<RuleConditionDb>>(!string.IsNullOrEmpty(node.RuleConditions)? node.RuleConditions: "[]");
            if (nodeConditions.Count <= 0) return ruleGroupColumn;
            foreach (var nodeCondition in nodeConditions)
            {
                ruleGroupColumn.Columns.Add(new StateEngineRuleCondtion
                {
                    Comparison = nodeCondition.Comparison,
                    ComparisonType = nodeCondition.ComparisonType,
                    ConditionName = nodeCondition.ConditionName,
                    ConditionID = nodeCondition.ConditionID,
                    RuleConditionID = nodeCondition.RuleConditionID,
                    ObjectTypeID = nodeCondition.ObjectTypeID,
                    RuleGroupID = nodeCondition.RuleGroupID,
                    StaticValue = nodeCondition.StaticValue,
                    ValueID = nodeCondition.ValueID,
                    ValueName = nodeCondition.ValueName
                });
            }
            return ruleGroupColumn;
        }
    }
}
