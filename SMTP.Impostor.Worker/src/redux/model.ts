import { IStatus } from './status';

export interface IAppState {
  status: IStatus;
}

export interface IAction {
  type: string;
  sendToHub?: true;
  model?: any;
}
