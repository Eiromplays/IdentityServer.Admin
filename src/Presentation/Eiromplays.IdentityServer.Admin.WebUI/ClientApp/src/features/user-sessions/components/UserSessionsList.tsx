import { useSearch, MatchRoute } from '@tanstack/react-location';
import { formatDate, Link, PaginatedTable, Spinner, useAuth } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { SearchUserSessionDTO } from '../api/searchUserSessions';
import { UserSession } from '../types';

import { DeleteUserSession } from './DeleteUserSession';

export const UserSessionsList = () => {
  const { user } = useAuth();
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || 1;
  const pageSize = search.pagination?.size || 10;

  if (!user) return null;

  return (
    <PaginatedTable<SearchUserSessionDTO, UserSession>
      url="/user-sessions/search"
      queryKeyName="search-user-sessions"
      searchData={{ pageNumber: page, pageSize: pageSize }}
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