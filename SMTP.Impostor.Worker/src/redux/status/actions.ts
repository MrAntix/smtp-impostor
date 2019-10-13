import { IHubMessage } from '../../impostor-hub';
import { IHost } from './model';
import {
  Types,
  GetStatus,
  Status,
  AddHost,
  RemoveHost,
  StartHost,
  StopHost
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

export const addHost = (host?: IHost) => dispatch => {
  const action: AddHost = {
    type: Types.ADD_HOST,
    sendToHub: true,
    model: host
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
