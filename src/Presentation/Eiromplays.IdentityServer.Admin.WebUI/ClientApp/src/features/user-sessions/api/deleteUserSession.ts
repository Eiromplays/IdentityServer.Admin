import { axios, MutationConfig, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { UserSession } from '../types';

export const deleteUserSession = ({ userSessionKey }: { userSessionKey: string }) => {
  return axios.delete(`/user-sessions/${userSessionKey}`);
};

type UseDeleteUserSessionOptions = {
  config?: MutationConfig<typeof deleteUserSession>;
};

export const useDeleteUserSession = ({ config }: UseDeleteUserSessionOptions = {}) => {
  return useMutation({
    onMutate: async (deletedUserSession) => {
      await queryClient.cancelQueries('user-sessions');

      const previousUserSessions = queryClient.getQueryData<UserSession[]>('user-sessions');

      queryClient.setQueryData(
        'user-sessions',
        previousUserSessions?.filter(
          (userSession) => userSession.key !== deletedUserSession.userSessionKey
        )
      );

      return { previousUserSessions };
    },
    onError: (_, __, context: any) => {
      if (context?.previousUserSessions) {
        queryClient.setQueryData('user-sessions', context.previousUserSessions);
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries('user-sessions');
      toast.success('User Session Deleted');
      window.location.href = '/app';
    },
    ...config,
    mutationFn: deleteUserSession,
  });
};
