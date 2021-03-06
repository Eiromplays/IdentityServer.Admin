import { Table, Spinner, Link, formatDate } from 'eiromplays-ui';

import { useGrants } from '../api/getGrants';
import { Grant } from '../types';

import { RevokeGrant } from './RevokeGrant';

export const GrantsList = () => {
  const grantsQuery = useGrants();

  if (grantsQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!grantsQuery?.data) return null;

  return (
    <Table<Grant>
      data={grantsQuery.data}
      columns={[
        {
          title: 'Client Id',
          field: 'clientId',
        },
        {
          title: 'Client Name',
          field: 'clientName',
        },
        {
          title: 'Description',
          field: 'description',
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
          field: 'clientId',
          Cell({ entry: { clientId } }) {
            return <Link to={`./${clientId}`}>View</Link>;
          },
        },
        {
          title: '',
          field: 'clientId',
          Cell({ entry: { clientId } }) {
            return <RevokeGrant clientId={clientId} />;
          },
        },
      ]}
    />
  );
};
