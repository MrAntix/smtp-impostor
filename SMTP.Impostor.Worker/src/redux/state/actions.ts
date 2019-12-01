import { IHubMessage } from '../../impostor-hub';
import { IHost, IHostUpdate, ISearchHostMessagesCriteria } from './model';
import {
  Types,
  LoadWorkerState,
  WorkerState,
  AddHost,
  RemoveHost,
  StartHost,
  StopHost,
  UpdateHost,
  ToggleHostConfiguration,
  ToggleHostMessages,
  SearchHostMessages,
  DeleteHostMessage,
  LoadHostMessage,
  ShutdownWorker
} from './types';
import { getInitialState } from './reducer';

export const dispatch = (action: IHubMessage) => dispatch => {
  dispatch(action);
};

export const initWorkerState = () => dispatch => {
  const action: WorkerState = {
    type: Types.WORKER_STATE,
    model: getInitialState()
  };
  dispatch(action);
};

export const loadWorkerState = () => dispatch => {
  const action: LoadWorkerState = {
    type: Types.LOAD_WORKER_STATE,
    sendToHub: true
  };
  dispatch(action);
};

export const addHost = (model?: IHost) => dispatch => {
  const action: AddHost = {
    type: Types.ADD_HOST,
    sendToHub: true,
    model
  };
  dispatch(action);
};

export const updateHost = (model: IHostUpdate) => dispatch => {
  const action: UpdateHost = {
    type: Types.UPDATE_HOST,
    sendToHub: true,
    model
  };
  dispatch(action);
};

export const removeHost = (hostId: string) => dispatch => {
  const action: RemoveHost = {
    type: Types.REMOVE_HOST,
    sendToHub: true,
    model: { hostId }
  };
  dispatch(action);
};

export const startHost = (hostId: string) => dispatch => {
  const action: StartHost = {
    type: Types.START_HOST,
    sendToHub: true,
    model: { hostId }
  };
  dispatch(action);
};

export const stopHost = (hostId: string) => dispatch => {
  const action: StopHost = {
    type: Types.STOP_HOST,
    sendToHub: true,
    model: { hostId }
  };
  dispatch(action);
};

export const toggleHostConfiguration = (host: IHost, value?: boolean) => dispatch => {
  const action: ToggleHostConfiguration = {
    type: Types.TOGGLE_HOST_CONFIGURATION,
    model: {
      hostId: host.id,
      value: value == null ? !host.showConfiguration : !!value
    }
  };
  dispatch(action);
};

export const toggleHostMessages = (host: IHost, value?: boolean) => dispatch => {
  const action: ToggleHostMessages = {
    type: Types.TOGGLE_HOST_MESSAGES,
    model: {
      hostId: host.id,
      value: value == null ? !host.showMessages : !!value
    }
  };
  dispatch(action);
};

export const searchHostMessages = (hostId: string, criteria: ISearchHostMessagesCriteria) => dispatch => {
  const action: SearchHostMessages = {
    type: Types.SEARCH_HOST_MESSAGES,
    sendToHub: true,
    model: {
      hostId,
      criteria
    }
  };
  dispatch(action);
};

export const deleteHostMessage = (hostId: string, messageId: string) => dispatch => {
  const action: DeleteHostMessage = {
    type: Types.DELETE_HOST_MESSAGE,
    sendToHub: true,
    model: {
      hostId,
      messageId
    }
  };
  dispatch(action);
};

export const loadHostMessage = (hostId: string, messageId: string) => dispatch => {
  const action: LoadHostMessage = {
    type: Types.LOAD_HOST_MESSAGE,
    sendToHub: true,
    model: {
      hostId,
      messageId
    }
  };
  dispatch(action);
};

export const shutdownWorker = () => dispatch => {
  const action: ShutdownWorker = {
    type: Types.SHUTDOWN_WORKER,
    sendToHub: true
  };
  dispatch(action);
};
