export const identityServerUrl: string =
  process.env.REACT_APP_IDENTITY_SERVER_URL ||
  process.env.VUE_APP_IDENTITY_SERVER_URL ||
  import.meta.env.VITE_IDENTITY_SERVER_URL ||
  'https://localhost:7001';

export const identityServerUiUrl: string =
  process.env.REACT_APP_IDENTITY_SERVER_UI_URL ||
  process.env.VUE_APP_IDENTITY_SERVER_UI_URL ||
  import.meta.env.VITE_IDENTITY_SERVER_UI_URL ||
  'https://localhost:3000';
