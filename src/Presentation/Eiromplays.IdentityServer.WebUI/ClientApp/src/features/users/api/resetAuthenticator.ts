import { axios, MutationConfig } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export const resetAuthenticator = () => {
  return axios.post(`https://localhost:7001/account/ResetAuthenticator`);
};

type UseResetAuthenticatorOptions = {
  config?: MutationConfig<typeof resetAuthenticator>;
};

export const useResetAuthenticator = ({ config }: UseResetAuthenticatorOptions = {}) => {
  return useMutation({
    onSuccess: () => {
      toast.success('Successfully reset authentication app key.');
    },
    ...config,
    mutationFn: resetAuthenticator,
  });
};