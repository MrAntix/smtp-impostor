import { IHost, IHostUpdate } from './model';

export enum Types {
  NULL = 'NULL',
  GET_STATUS = 'GetStatus',
  STATUS = 'Status',
  ADD_HOST = 'AddHost',
  REMOVE_HOST = 'RemoveHost',
  START_HOST = 'Starthost',
  STOP_HOST = 'StopHost',
  UPDATE_HOST = 'UpdateHost',
  HOST_UPDATED = 'HostUpdated',
  TOGGLE_HOST_CONFIGURATION = 'ToggleHostConfiguration',
  TOGGLE_HOST_MESSAGES = 'ToggleHostMessages'
}

export interface NullAction {
  type: Types.NULL;
}

export interface GetStatus {
  type: Types.GET_STATUS;
  sendToHub: true;
}

export interface Status {
  type: Types.STATUS;
  model: {
    hosts: IHost[];
    fileStorePath: string;
  };
}

export interface AddHost {
  type: Types.ADD_HOST;
  sendToHub: true;
  model: IHost;
}

export interface UpdateHost {
  type: Types.UPDATE_HOST;
  sendToHub: true;
  model: IHostUpdate;
}

export interface HostUpdated {
  type: Types.HOST_UPDATED;
  host: IHost;
}

export interface RemoveHost {
  type: Types.REMOVE_HOST;
  sendToHub: true;
  model: { hostId: string };
}

export interface StartHost {
  type: Types.START_HOST;
  sendToHub: true;
  model: { hostId: string };
}

export interface StopHost {
  type: Types.STOP_HOST;
  sendToHub: true;
  model: { hostId: string };
}

export interface ToggleHostConfiguration {
  type: Types.TOGGLE_HOST_CONFIGURATION;
  model: { hostId: string, value: boolean };
}

export interface ToggleHostMessages {
  type: Types.TOGGLE_HOST_MESSAGES;
  model: { hostId: string, value: boolean };
}

export type ActionTypes =
  | NullAction
  | Status
  | HostUpdated
  | ToggleHostConfiguration
  | ToggleHostMessages;
