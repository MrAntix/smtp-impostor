import { IHost } from './model';

export enum TypeKeys {
  NULL = 'NULL',
  STATUS = 'Status',
  START = 'START',
  STOP = 'STOP',
  UPDATED = 'UPDATED'
}

export interface NullAction {
  type: TypeKeys.NULL;
}

export interface Status {
  type: TypeKeys.STATUS;
  model: {
    hosts: IHost[];
    fileStorePath: string;
  };
}

export interface HostStart {
  type: TypeKeys.START;
  hostId: string;
}
export const startHost = (hostId: string) => dispatch => {
  const action: HostStart = {
    type: TypeKeys.START,
    hostId
  };
  dispatch(action);
};

export interface HostStop {
  type: TypeKeys.STOP;
  hostId: string;
}
export const stopHost = (hostId: string) => dispatch => {
  const action: HostStop = {
    type: TypeKeys.STOP,
    hostId
  };
  dispatch(action);
};

export interface HostUpdated {
  type: TypeKeys.UPDATED;
  host: IHost;
}

export type ActionTypes =
  | NullAction
  | Status
  | HostStart
  | HostStop
  | HostUpdated;
