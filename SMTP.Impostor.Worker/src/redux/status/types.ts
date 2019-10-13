import { IHost } from './model';

export enum Types {
  NULL = 'NULL',
  GET_STATUS = 'GetStatus',
  STATUS = 'Status',
  ADD_HOST = 'AddHost',
  REMOVE_HOST = 'RemoveHost',
  START_HOST = 'Starthost',
  STOP_HOST = 'StopHost',
  UPDATED = 'UPDATED'
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

export interface HostUpdated {
  type: Types.UPDATED;
  host: IHost;
}

export type ActionTypes =
  | NullAction
  | Status
  | HostUpdated;
