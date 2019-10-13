export interface IStatus {
  hosts: IHost[];
  fileStorePath: string;
}

export interface IHost {
  id?: string;
  name?: string;
  ip?: string;
  port?: number;
  state?: HostStatus;
  start: boolean;
}

export enum HostStatus {
  Stopped,
  Errored,
  Started,
  Receiving
}
