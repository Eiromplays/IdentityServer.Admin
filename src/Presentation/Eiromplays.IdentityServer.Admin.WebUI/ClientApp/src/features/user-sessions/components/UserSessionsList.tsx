import { useSearch, MatchRoute } from '@tanstack/react-location';
import {
  defaultPageIndex,
  defaultPageSize,
  formatDate,
  Link,
  PaginatedTable,
  SearchFilter,
  Spinner,
  useAuth,
} from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { SearchUserSessionDTO } from '../api/searchUserSessions';
import { UserSession } from '../types';

import { DeleteUserSession } from './DeleteUserSession';

export const UserSessionsList = () => {
  const { user } = useAuth();
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || defaultPageIndex;
  const pageSize = search.pagination?.size || defaultPageSize;

  if (!user) return null;

  const searchFilter: SearchFilter = {
    customProperties: [],
    orderBy: ['id', 'subjectId', 'sessionId', 'applicationName'],
    advancedSearch: {
      fields: ['id', 'subjectId', 'sessionId', 'applicationName'],
      keyword: '',
    },
    keyword: '',
  };

  return (
    <PaginatedTable<SearchUserSessionDTO, UserSession>
      url="/user-sessions/search"
      queryKeyName="search-user-sessions"
      searchData={{ pageNumber: page, pageSize: pageSize }}
      searchFilter={searchFilter}
      columns={[
        {
          title: 'Session Id',
          field: 'sessionId',
          Cell({ entry: { sessionId } }) {
            return (
              <span>
                {user.sessionId === sessionId && (
                  <span className="font-bold text-1xl">(Current)</span>
                )}{' '}
                {sessionId}
              </span>
            );
          },
        },
        {
          title: 'Created At',
          field: 'created',
          Cell({ entry: { created } }) {
            return <span>{formatDate(created)}</span>;
          },
        },
        {
          title: 'Expires At',
          field: 'expires',
          Cell({ entry: { expires } }) {
            return <span>{formatDate(expires)}</span>;
          },
        },
        {
          title: '',
          field: 'key',
          Cell({ entry: { key } }) {
            return (
              <Link to={key} search={search} className="block">
                <pre className={`text-sm`}>
                  View{' '}
                  <MatchRoute to={key} pending>
                    <Spinner size="md" className="inline-block" />
                  </MatchRoute>
                </pre>
              </Link>
            );
          },
        },
        {
          title: '',
          field: 'key',
          Cell({ entry: { key, sessionId } }) {
            return (
              <DeleteUserSession
                userSessionKey={key}
                currentSession={user.sessionId === sessionId}
              />
            );
          },
        },
      ]}
    />
  );
};
