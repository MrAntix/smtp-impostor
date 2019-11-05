import { IStatus, IHost } from './model';
import { ActionTypes, Types } from './types';

export const getInitialState = (): IStatus => ({
  hosts: null,
  fileStorePath: null
});

export default (state = getInitialState(), action: ActionTypes): IStatus => {
  console.log('status.reducer', action);
  switch (action.type) {
    default: return state;

    case Types.STATUS:
      return action.model;
    case Types.TOGGLE_HOST_CONFIGURATION:
      return updateHost(state,
        action.model.hostId, {
        showConfiguration: action.model.value
      })
    case Types.TOGGLE_HOST_MESSAGES:
      return updateHost(state,
        action.model.hostId, {
        showMessages: action.model.value
      })
    case Types.HOST_MESSAGES_LOADED:
      return updateHost(state,
        action.model.hostId, {
        messagesIndex: action.model.result.index,
        messagesCount: action.model.result.total,
        messages: action.model.result.messages
      })
  }
};

function updateHost(
  state: IStatus,
  hostId: string, update: Partial<IHost>): IStatus {
  return {
    ...state,
    hosts: state.hosts.map(host => host.id === hostId
      ? { ...host, ...update }
      : host)
  }
}
