import { IHost } from './model';

export enum Types {
  NULL = 'NULL',
  STATUS = 'Status',
  START = 'START',
  STOP = 'STOP',
  UPDATED = 'UPDATED'
}

export interface NullAction {
  type: Types.NULL;
}

export interface Status {
  type: Types.STATUS;
  model: {
    hosts: IHost[];
    fileStorePath: string;
  };
}

export interface HostStart {
  type: Types.START;
  hostId: string;
}
export const startHost = (hostId: string) => dispatch => {
  const action: HostStart = {
    type: Types.START,
    hostId
  };
  dispatch(action);
};

export interface HostStop {
  type: Types.STOP;
  hostId: string;
}
export const stopHost = (hostId: string) => dispatch => {
  const action: HostStop = {
    type: Types.STOP,
    hostId
  };
  dispatch(action);
};

export interface HostUpdated {
  type: Types.UPDATED;
  host: IHost;
}

export type ActionTypes =
  | NullAction
  | Status
  | HostStart
  | HostStop
  | HostUpdated;
