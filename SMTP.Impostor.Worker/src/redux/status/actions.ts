import { IHubMessage } from '../../impostor-hub';
import { IHost, IHostUpdate, ISearchHostMessagesCriteria } from './model';
import {
  Types,
  GetStatus,
  Status,
  AddHost,
  RemoveHost,
  StartHost,
  StopHost,
  UpdateHost,
  ToggleHostConfiguration,
  ToggleHostMessages,
  SearchHostMessages
} from './types';
import { getInitialState } from './reducer';

export const dispatch = (action: IHubMessage) => dispatch => {
  dispatch(action);
};

export const initStatus = () => dispatch => {
  const action: Status = {
    type: Types.STATUS,
    model: getInitialState()
  };
  dispatch(action);
};

export const getStatus = () => dispatch => {
  const action: GetStatus = {
    type: Types.GET_STATUS,
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

export const searchHostMessages = (host: IHost, criteria: ISearchHostMessagesCriteria) => dispatch => {
  const action: SearchHostMessages = {
    type: Types.SEARCH_HOST_MESSAGES,
    sendToHub: true,
    model: {
      hostId: host.id,
      criteria
    }
  };
  dispatch(action);
};
