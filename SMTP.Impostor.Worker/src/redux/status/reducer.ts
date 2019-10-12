import { IStatus } from './model';
import { ActionTypes, Types } from './actions';

export const getInitialState = (): IStatus => ({
  hosts: [],
  fileStorePath: null
});

export default (state = getInitialState(), action: ActionTypes): IStatus => {
  console.log('status.reducer', action);
  switch (action.type) {
    case Types.STATUS:
      return action.model;
  }
  return state;
};
