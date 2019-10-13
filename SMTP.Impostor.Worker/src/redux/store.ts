import { createStore, applyMiddleware, Middleware } from 'redux';
import thunk from 'redux-thunk';
import { composeWithDevTools } from 'redux-devtools-extension/developmentOnly';

import rootReducer from './reducer';
import { IAppState } from './model';

export const configureStore = (
  preloadedState: Partial<IAppState>,
  hubAction: Middleware
) =>
  createStore(
    rootReducer,
    preloadedState,
    composeWithDevTools(applyMiddleware(thunk, hubAction))
  );
