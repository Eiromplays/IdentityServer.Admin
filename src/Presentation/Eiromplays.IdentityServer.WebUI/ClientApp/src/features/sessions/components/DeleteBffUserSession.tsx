import { Button, ConfirmationDialog } from 'eiromplays-ui';
import { HiOutlineTrash } from 'react-icons/hi';

import { useDeleteBffUserSession } from '../api/deleteBffUserSession';

type DeleteUserSessionProps = {
  userSessionKey: string;
  currentSession: boolean;
};

export const DeleteBffUserSession = ({
  userSessionKey,
  currentSession,
}: DeleteUserSessionProps) => {
  const deleteUserSessionMutation = useDeleteBffUserSession();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete User Session"
      body={`Are you sure you want to delete this user session? ${
        currentSession ? '(It will revoke access to this application)' : ''
      }`}
      triggerButton={
        <Button variant="danger" startIcon={<HiOutlineTrash className="h-4 w-4" />}>
          Delete
        </Button>
      }
      confirmButton={
        <Button
          isLoading={deleteUserSessionMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={async () =>
            await deleteUserSessionMutation.mutateAsync({
              currentSession: currentSession,
              userSessionKey: userSessionKey,
            })
          }
        >
          Delete User Session
        </Button>
      }
    />
  );
};
