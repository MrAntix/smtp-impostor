import { combineReducers } from 'redux';

import status from './status/reducer';

export const rootReducer = combineReducers({
  status
});

export default rootReducer;
