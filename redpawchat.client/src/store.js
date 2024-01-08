import { configureStore } from '@reduxjs/toolkit';
import { reducer as conversationReducer } from './conversationSlice';

const store = configureStore({
  reducer: {
    conversation: conversationReducer,

  },
});

export default store;