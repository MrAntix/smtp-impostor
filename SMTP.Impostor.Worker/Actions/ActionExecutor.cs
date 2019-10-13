using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SMTP.Impostor.Worker.Actions
{
    public class ActionExecutor : IActionExecutor
    {
        readonly IImmutableDictionary<string, Tuple<Type, IAction>> _actions;

        public ActionExecutor(
            IEnumerable<IAction> actions)
        {
            _actions = actions.Select(action =>
            {
                var actionType = action.GetType();
                var requestType = action.RequestType;
                return KeyValuePair.Create(
                      ActionBase.GetName(actionType),
                      Tuple.Create(requestType, action)
                      );
            }
            ).ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);
        }

        async Task<object> IActionExecutor.ExecuteAsync(
            string actionName, string data)
        {
            if (!_actions.ContainsKey(actionName))
                return ActionNull.Instance;

            var actionInfo = _actions[actionName];
            var requestType = actionInfo.Item1;
            var action = actionInfo.Item2;

            var request = JsonConvert
                .DeserializeObject(data ?? "{}", requestType);

            return await action.ExecuteAsync(request);
        }
    }
}
