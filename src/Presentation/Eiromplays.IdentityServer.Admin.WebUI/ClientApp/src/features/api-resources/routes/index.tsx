import { Navigate, Route } from '@tanstack/react-location';
import { defaultPageIndex, defaultPageSize, queryClient, searchPagination } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getApiResource } from '../api/getApiResource';

import { ApiResourceInfo } from './ApiResourceInfo';
import { ApiResources } from './ApiResources';

export const ApiResourcesRoutes: Route<LocationGenerics> = {
  path: 'api-resources',
  children: [
    {
      path: '/',
      element: <ApiResources />,
      loader: async ({ search: { pagination, searchFilter } }) =>
        (await queryClient.getQueryData([
          'api-resources',
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ])) ??
        (await queryClient.fetchQuery(
          [
            'api-resources',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          () =>
            searchPagination(
              '/api-resources/search',
              {
                pageNumber: pagination?.index || defaultPageIndex,
                pageSize: pagination?.size || defaultPageSize,
              },
              searchFilter
            )
        )),
    },
    {
      path: ':apiResourceId',
      element: <ApiResourceInfo />,
      loader: async ({ params: { apiResourceId } }) =>
        queryClient.getQueryData(['api-resource', apiResourceId]) ??
        (await queryClient.fetchQuery(['api-resource', apiResourceId], () =>
          getApiResource({ apiResourceId: parseInt(apiResourceId, 10) })
        )),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
