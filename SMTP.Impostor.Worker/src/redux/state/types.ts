import {
  IHost, IHostUpdate,
  ISearchHostMessagesCriteria, IHostMesssageInfo
} from './model';

export enum Types {
  NULL = 'NULL',
  LOAD_WORKER_STATE = 'LoadWorkerState',
  WORKER_STATE = 'WorkerState',
  ADD_HOST = 'AddHost',
  REMOVE_HOST = 'RemoveHost',
  START_HOST = 'Starthost',
  STOP_HOST = 'StopHost',
  UPDATE_HOST = 'UpdateHost',
  HOST_STATE = 'HostState',
  TOGGLE_HOST_CONFIGURATION = 'ToggleHostConfiguration',
  TOGGLE_HOST_MESSAGES = 'ToggleHostMessages',
  SEARCH_HOST_MESSAGES = 'SearchHostMessages',
  HOST_MESSAGES_LOADED = 'HostMessagesLoaded',
  HOST_MESSAGE_RECEIVED = 'HostMessageReceived',
  HOST_MESSAGE_ADDED = 'HostMessageAdded',
  HOST_MESSAGE_REMOVED = 'HostMessageRemoved',
  DELETE_HOST_MESSAGE = 'DeleteHostMessage',
  LOAD_HOST_MESSAGE = 'LoadHostMessage',
  HOST_MESSAGE = 'HostMessage'
}

export interface NullAction {
  type: Types.NULL;
}

export interface LoadWorkerState {
  type: Types.LOAD_WORKER_STATE;
  sendToHub: true;
}

export interface WorkerState {
  type: Types.WORKER_STATE;
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

export interface HostState {
  type: Types.HOST_STATE;
  model: IHost;
}

export interface ToggleHostConfiguration {
  type: Types.TOGGLE_HOST_CONFIGURATION;
  model: { hostId: string, value: boolean };
}

export interface ToggleHostMessages {
  type: Types.TOGGLE_HOST_MESSAGES;
  model: { hostId: string, value: boolean };
}

export interface SearchHostMessages {
  type: Types.SEARCH_HOST_MESSAGES;
  sendToHub: true;
  model: {
    hostId: string,
    criteria: ISearchHostMessagesCriteria
  }
}

export interface DeleteHostMessage {
  type: Types.DELETE_HOST_MESSAGE;
  sendToHub: true;
  model: {
    hostId: string,
    messageId: string
  }
}

export interface LoadHostMessage {
  type: Types.LOAD_HOST_MESSAGE;
  sendToHub: true;
  model: {
    hostId: string,
    messageId: string
  }
}

export interface HostMessagesLoaded {
  type: Types.HOST_MESSAGES_LOADED;
  model: {
    hostId: string,
    index: number;
    total: number;
    messages: IHostMesssageInfo[];
  }
}

export interface HostMessageReceived {
  type: Types.HOST_MESSAGE_RECEIVED;
  model: {
    hostId: string,
    date: Date,
    from: string,
    subject: string
  }
}

export interface HostMessageAdded {
  type: Types.HOST_MESSAGE_ADDED;
  model: {
    hostId: string,
    messageId: string
  }
}

export interface HostMessageRemoved {
  type: Types.HOST_MESSAGE_REMOVED;
  model: {
    hostId: string,
    messageId: string
  }
}

export interface HostMessage {
  type: Types.HOST_MESSAGE;
  model: {
    hostId: string,
    id: string,
    date: Date,
    from: string,
    subject: string,
    content: string
  }
}

export type ActionTypes =
  | NullAction
  | WorkerState
  | HostState
  | ToggleHostConfiguration
  | ToggleHostMessages
  | HostMessagesLoaded
  | HostMessageAdded
  | HostMessageRemoved
  | HostMessage;
