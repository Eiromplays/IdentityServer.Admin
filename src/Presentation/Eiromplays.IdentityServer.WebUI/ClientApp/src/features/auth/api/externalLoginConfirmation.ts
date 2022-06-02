import { axios, MessageResponse, MutationConfig } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { identityServerUrl } from '@/utils/envVariables';

export type ExternalLoginConfirmationViewModel = {
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  phoneNumber?: string;
};

export type ExternalLoginConfirmationResponse = MessageResponse & {
  returnUrl: string;
};

export const externalLoginConfirmation = ({
  returnUrl,
  data,
}: {
  data: ExternalLoginConfirmationViewModel;
  returnUrl?: string;
}): Promise<ExternalLoginConfirmationResponse> => {
  return axios.post(
    `${identityServerUrl}/spa/externalLoginConfirmation?returnUrl=${returnUrl}`,
    data
  );
};

type UseRevokeGrantOptions = {
  config?: MutationConfig<typeof externalLoginConfirmation>;
};

export const useExternalLoginConfirmation = ({ config }: UseRevokeGrantOptions = {}) => {
  return useMutation({
    onSuccess: (response) => {
      toast.success('Successfully created account');
      toast.success(response.message);
    },
    ...config,
    mutationFn: externalLoginConfirmation,
  });
};
