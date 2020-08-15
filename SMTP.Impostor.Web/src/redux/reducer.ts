import { combineReducers } from 'redux';

import worker from './state/reducer';

export const rootReducer = combineReducers({
  worker
});

export default rootReducer;
