import { IWorkerState } from './state';

export interface IAppState {
  worker: IWorkerState;
}

export interface IAction {
  type: string;
  sendToHub?: true;
  model?: any;
}
