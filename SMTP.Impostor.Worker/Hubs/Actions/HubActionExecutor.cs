using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SMTP.Impostor.Worker.Hubs.Actions
{
    public class HubActionExecutor : IHubActionExecutor
    {
        readonly IImmutableDictionary<string, Tuple<Type, IHubAction>> _actions;

        public HubActionExecutor(
            IEnumerable<IHubAction> actions)
        {
            _actions = actions.Select(action =>
            {
                var actionType = action.GetType();
                var requestType = action.RequestType;
                return KeyValuePair.Create(
                      actionType.Name[0..^6],
                      Tuple.Create(requestType, action)
                      );
            }
            ).ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);
        }

        async Task<object> IHubActionExecutor.ExecuteAsync(
            string actionName, string data)
        {
            if (!_actions.ContainsKey(actionName))
                return NullActionResponse.Instance;

            var actionInfo = _actions[actionName];
            var requestType = actionInfo.Item1;
            var action = actionInfo.Item2;

            var request = JsonConvert
                .DeserializeObject(data ?? "{}", requestType);

            return await action.ExecuteAsync(request);
        }
    }
}
