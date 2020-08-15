import { IWorkerState, IHost } from './model';
import { ActionTypes, Types } from './types';

export const getInitialState = (): IWorkerState => ({
  hosts: null,
  openHostId: null,
  fileStorePath: null
});

export default (state = getInitialState(), action: ActionTypes): IWorkerState => {
  switch (action.type) {
    default: return state;

    case Types.WORKER_STATE:
      return {
        ...action.model,
        openHostId: state.openHostId
      };

    case Types.HOST_STATE:
      return updateHost(state,
        action.model.id,
        () => action.model);

    case Types.TOGGLE_HOST_CONFIGURATION:
      return updateHost(state,
        action.model.hostId,
        () => ({
          showConfiguration: action.model.value
        }));

    case Types.OPEN_HOST:
      return {
        ...state,
        openHostId: action.model.hostId
      }

    case Types.HOST_MESSAGES_LOADED:
      return updateHost(state,
        action.model.hostId,
        () => ({
          messagesIndex: action.model.index,
          messagesCount: action.model.total,
          messages: action.model.messages
        }));

    case Types.HOST_MESSAGE_REMOVED:
      return updateHost(state,
        action.model.hostId,
        host => ({
          messagesCount: host.messagesCount - 1,
          messages: host.messages.filter(m => m.id != action.model.messageId)
        }));
  }
};

function updateHost(
  state: IWorkerState,
  hostId: string, update: (host: IHost) => Partial<IHost>): IWorkerState {
  return {
    ...state,
    hosts: state.hosts.map(host => host.id === hostId
      ? { ...host, ...update(host) }
      : host)
  }
}
