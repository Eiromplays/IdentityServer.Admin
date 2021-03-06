import { useAuth, axios, MutationConfig, MessageResponse } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type ChangePasswordDTO = {
  data: {
    userId?: string;
    password?: string;
    newPassword: string;
    confirmNewPassword: string;
  };
};

export const changePassword = async ({ data }: ChangePasswordDTO): Promise<MessageResponse> => {
  return axios.put(`/personal/change-password`, data);
};

type UseChangePasswordOptions = {
  config?: MutationConfig<typeof changePassword>;
};

export const useChangePassword = ({ config }: UseChangePasswordOptions = {}) => {
  const { logout } = useAuth();

  return useMutation({
    onSuccess: async (response) => {
      toast.success('Password Updated');
      toast.success(response.message);
      await logout();
    },
    onError: (error) => {
      toast.error('Failed to update password');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: changePassword,
  });
};
