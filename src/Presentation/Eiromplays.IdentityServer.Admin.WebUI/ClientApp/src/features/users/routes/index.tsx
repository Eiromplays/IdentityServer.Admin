import { Navigate, Route } from '@tanstack/react-location';
import { defaultPageIndex, defaultPageSize, queryClient, searchPagination } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { getUser } from '../api/getUser';
import { getUserRoles } from '../api/getUserRoles';
import { UserClaims } from '../routes/UserClaims';

import { User } from './User';
import { UserProviders } from './UserProviders';
import { UserRoles } from './UserRoles';
import { Users } from './Users';

export const UsersRoutes: Route<LocationGenerics> = {
  path: 'users',
  children: [
    {
      path: '/',
      element: <Users />,
      loader: async ({ search: { pagination, searchFilter } }) => {
        return (
          (await queryClient.getQueryData([
            'search-users',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ])) ??
          (await queryClient.fetchQuery(
            [
              'search-users',
              pagination?.index || defaultPageIndex,
              pagination?.size || defaultPageSize,
            ],
            () =>
              searchPagination(
                '/users/search',
                {
                  pageNumber: pagination?.index || defaultPageIndex,
                  pageSize: pagination?.size || defaultPageSize,
                },
                searchFilter
              )
          ))
        );
      },
    },
    {
      path: ':userId/roles',
      element: <UserRoles />,
      loader: async ({ params: { userId } }) =>
        queryClient.getQueryData(['user', userId, 'roles']) ??
        (await queryClient.fetchQuery(['user', userId, 'roles'], () =>
          getUserRoles({ userId: userId })
        )),
    },
    {
      path: ':userId/claims',
      element: <UserClaims />,
      loader: async ({ params: { userId }, search: { pagination, searchFilter } }) => {
        return (
          (await queryClient.getQueryData([
            `search-user-claims-${userId}`,
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ])) ??
          (await queryClient.fetchQuery(
            [
              `search-user-claims-${userId}`,
              pagination?.index || defaultPageIndex,
              pagination?.size || defaultPageSize,
            ],
            () =>
              searchPagination(
                `/users/${userId}/claims-search`,
                {
                  pageNumber: pagination?.index || defaultPageIndex,
                  pageSize: pagination?.size || defaultPageSize,
                },
                searchFilter
              )
          ))
        );
      },
    },
    {
      path: ':userId/providers',
      element: <UserProviders />,
      loader: async ({ params: { userId }, search: { pagination, searchFilter } }) => {
        return (
          (await queryClient.getQueryData([
            `search-user-providers-${userId}`,
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ])) ??
          (await queryClient.fetchQuery(
            [
              `search-user-providers-${userId}`,
              pagination?.index || defaultPageIndex,
              pagination?.size || defaultPageSize,
            ],
            () =>
              searchPagination(
                `/users/${userId}/providers-search`,
                {
                  pageNumber: pagination?.index || defaultPageIndex,
                  pageSize: pagination?.size || defaultPageSize,
                },
                searchFilter
              )
          ))
        );
      },
    },
    {
      path: ':userId',
      element: <User />,
      loader: async ({ params: { userId } }) =>
        queryClient.getQueryData(['user', userId]) ??
        (await queryClient.fetchQuery(['user', userId], () => getUser({ userId: userId }))),
    },
    {
      path: '*',
      element: <Navigate to="." />,
    },
  ],
};
