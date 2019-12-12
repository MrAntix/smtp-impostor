export interface IWorkerState {
  hosts: IHost[];
  openHostId?: string,
  fileStorePath: string;
}

export interface IHost {
  id?: string;
  name?: string;
  ip?: string;
  port?: number;
  state?: HostStatus;
  start: boolean;
  showConfiguration: boolean;
  messages: IHostMesssageInfo[];
  messagesIndex: number;
  messagesCount: number;
  maxMessages: number;
}

export interface IHostMesssageInfo {
  id: string;
  date: Date;
  from: string;
  subject: string;
  to: string[];
  attachments: string[];
}

export interface IHostUpdate {
  id?: string;
  name?: string;
  ip?: string;
  port?: number;
  maxMessages?: number;
}

export interface ISearchHostMessagesCriteria {
  text: string;
  index: number;
  count: number;
}

export const DEFAULT_SEARCH_HOST_MESSAGES_CRITERIA: ISearchHostMessagesCriteria = {
  text: '',
  index: 0,
  count: 20
}

export enum HostStatus {
  Stopped,
  Errored,
  Started,
  Receiving
}
