import { ContentLayout } from '@/components/Layout';
import { useAuth } from '@/lib/auth';

import { usePersonalData } from '../api/downloadPersonalData';

export const PersonalData = () => {
  const { user } = useAuth();
  const { downloadPersonalDataMutation } = usePersonalData();

  if (!user) return null;

  return (
    <ContentLayout title="Personal Data">
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <div className="flex justify-between">
            <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200">
              User management
            </h3>
          </div>
          <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
            Personal data and information.
          </p>
        </div>
        <div className="border-t border-gray-200 px-4 py-5 sm:p-0">
          <dl className="sm:divide-y sm:divide-gray-200">
            <button
              onClick={async () =>
                await downloadPersonalDataMutation.mutateAsync({ userId: user.id })
              }
            >
              Download Personal Data
            </button>
            <button>Delete Account</button>
          </dl>
        </div>
      </div>
    </ContentLayout>
  );
};
