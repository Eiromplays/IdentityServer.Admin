import { useSearch } from '@tanstack/react-location';
import { axios, MutationConfig, PaginationResponse, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';
import { IdentityResource } from '@/features/identity-resources';

export type DeleteIdentityResourceDTO = {
  identityResourceId: number;
};

export const deleteIdentityResource = ({ identityResourceId }: DeleteIdentityResourceDTO) => {
  return axios.delete(`/identity-resources/${identityResourceId}`);
};

type UseDeleteIdentityResourceOptions = {
  config?: MutationConfig<typeof deleteIdentityResource>;
};

export const useDeleteIdentityResource = ({ config }: UseDeleteIdentityResourceOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (deletedIdentityResource) => {
      await queryClient.cancelQueries(['search-identity-resources']);

      const previousIdentityResources = queryClient.getQueryData<
        PaginationResponse<IdentityResource>
      >(['search-identity-resources', pagination?.index ?? 1, pagination?.size ?? 10]);

      queryClient.setQueryData(
        ['search-identity-resources', pagination?.index ?? 1, pagination?.size ?? 10],
        previousIdentityResources?.data?.filter(
          (identityResource) => identityResource.id !== deletedIdentityResource.identityResourceId
        )
      );

      return { previousIdentityResources };
    },
    onError: (_, __, context: any) => {
      if (context?.previousIdentityResources) {
        queryClient.setQueryData(
          ['search-identity-resources', pagination?.index ?? 1, pagination?.size ?? 10],
          context.previousIdentityResources
        );
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries([
        'search-identity-resources',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);
      toast.success('IdentityResource deleted');
    },
    ...config,
    mutationFn: deleteIdentityResource,
  });
};
