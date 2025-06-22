import React from 'react';
import { Snackbar, Alert } from '@mui/material';

const Toast = ({ open, message, severity = 'info', onClose, autoHideDuration = 10000 }) => {
  return (
    <Snackbar
      open={open}
      autoHideDuration={autoHideDuration}
      anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
    >
      <Alert 
        onClose={onClose} 
        severity={severity} 
        variant="filled"
        sx={{ width: '100%' }}
      >
        {message}
      </Alert>
    </Snackbar>
  );
};

export default Toast; 