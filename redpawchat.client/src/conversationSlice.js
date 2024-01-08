import { createSlice } from '@reduxjs/toolkit';

export const conversationSlice = createSlice({
  name: 'conversation',
  initialState: {
    value: null,
  },
  reducers: {
    updateValue: (state, action) => {
      state.value = action.payload;
    },
}
});
export const { updateValue } = conversationSlice.actions;
export const { actions, reducer } = conversationSlice;
