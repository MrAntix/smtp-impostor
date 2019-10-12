import { IStatus } from './model';
import { ActionTypes, TypeKeys } from './actions';

const getInitialState = (): IStatus => {
  return {
    hosts: [],
    fileStorePath: null
  };
};

const status = (state = getInitialState(), action: ActionTypes): IStatus => {
  switch (action.type) {
    case TypeKeys.STATUS:
      return action.model;
  }
  return state;
};

export default status;
