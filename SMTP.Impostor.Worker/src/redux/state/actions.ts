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
  OpenHost,
  SearchHostMessages,
  DeleteHostMessage,
  LoadHostMessage,
  ShutdownWorker
} from './types';
import { getInitialState } from './reducer';
import { IDispatch } from '../model';

export const dispatch = (action: IHubMessage) => (dispatch: IDispatch) => {
  dispatch(action);
};

export const initWorkerState = () => (dispatch: IDispatch) => {
  const action: WorkerState = {
    type: Types.WORKER_STATE,
    model: getInitialState()
  };
  dispatch(action);
};

export const loadWorkerState = () => (dispatch: IDispatch) => {
  const action: LoadWorkerState = {
    type: Types.LOAD_WORKER_STATE,
    sendToHub: true
  };
  dispatch(action);
};

export const addHost = (model?: Partial<IHost>) => (dispatch: IDispatch) => {
  const action: AddHost = {
    type: Types.ADD_HOST,
    sendToHub: true,
    model
  };
  dispatch(action);
};

export const updateHost = (model: IHostUpdate) => (dispatch: IDispatch) => {
  const action: UpdateHost = {
    type: Types.UPDATE_HOST,
    sendToHub: true,
    model
  };
  dispatch(action);
};

export const removeHost = (hostId: string) => (dispatch: IDispatch) => {
  const action: RemoveHost = {
    type: Types.REMOVE_HOST,
    sendToHub: true,
    model: { hostId }
  };
  dispatch(action);
};

export const startHost = (hostId: string) => (dispatch: IDispatch) => {
  const action: StartHost = {
    type: Types.START_HOST,
    sendToHub: true,
    model: { hostId }
  };
  dispatch(action);
};

export const stopHost = (hostId: string) => (dispatch: IDispatch) => {
  const action: StopHost = {
    type: Types.STOP_HOST,
    sendToHub: true,
    model: { hostId }
  };
  dispatch(action);
};

export const toggleHostConfiguration = (hostId: string, value: boolean) => (dispatch: IDispatch) => {
  const action: ToggleHostConfiguration = {
    type: Types.TOGGLE_HOST_CONFIGURATION,
    model: {
      hostId,
      value
    }
  };
  dispatch(action);
};

export const openHost = (hostId: string) => (dispatch: IDispatch) => {
  const action: OpenHost = {
    type: Types.OPEN_HOST,
    model: {
      hostId
    }
  };
  dispatch(action);
};

export const searchHostMessages = (hostId: string, criteria: ISearchHostMessagesCriteria) => (dispatch: IDispatch) => {
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

export const deleteHostMessage = (hostId: string, messageId: string) => (dispatch: IDispatch) => {
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

export const loadHostMessage = (hostId: string, messageId: string) => (dispatch: IDispatch) => {
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

export const shutdownWorker = () => (dispatch: IDispatch) => {
  const action: ShutdownWorker = {
    type: Types.SHUTDOWN_WORKER,
    sendToHub: true
  };
  dispatch(action);
};
