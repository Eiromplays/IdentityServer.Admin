import { Outlet } from '@tanstack/react-location';
import { lazyImport, MainLayout, NotAllowed, Spinner } from 'eiromplays-ui';
import { Suspense } from 'react';
import { AiOutlineTags, BiTargetLock, MdOutlineVerified } from 'react-icons/all';
import {
  HiLockClosed,
  HiOutlineDocumentText,
  HiOutlineHome,
  HiOutlineShieldCheck,
  HiOutlineUsers,
} from 'react-icons/hi';
import { MdOutlineDevicesOther, MdOutlineHistory } from 'react-icons/md';

import logo from '@/assets/logo.svg';
import { ApiResourcesRoutes } from '@/features/api-resources';
import { ApiScopesRoutes } from '@/features/api-scopes';
import { ClientsRoutes } from '@/features/clients';
import { IdentityResourcesRoutes } from '@/features/identity-resources';
import { LogsRoutes } from '@/features/logs';
import { PersistedGrantsRoutes } from '@/features/persisted-grants';
import { RolesRoutes } from '@/features/roles';
import { UserSessionsRoutes } from '@/features/user-sessions';
import { UsersRoutes } from '@/features/users';
import { ROLES, useAuthorization } from '@/lib/authorization';
import { identityServerUiUrl, identityServerUrl } from '@/utils/envVariables';

const { Dashboard } = lazyImport(() => import('@/features/misc'), 'Dashboard');

const App = () => {
  const { checkAccess } = useAuthorization();

  if (!checkAccess({ allowedRoles: [ROLES.ADMINISTRATOR] })) return <NotAllowed logo={logo} />;

  return (
    <MainLayout
      logo={logo}
      userNavigationProps={{ identityServerUiUrl: identityServerUiUrl }}
      sideBarNavigationProps={{
        items: [
          { name: 'Dashboard', to: '.', icon: HiOutlineHome },
          { name: 'Users', to: './users', icon: HiOutlineUsers },
          { name: 'Roles', to: './roles', icon: HiLockClosed },
          { name: 'Clients', to: './clients', icon: MdOutlineDevicesOther },
          { name: 'Identity Resources', to: './identity-resources', icon: AiOutlineTags },
          { name: 'Api Scopes', to: './api-scopes', icon: BiTargetLock },
          { name: 'Api Resources', to: './api-resources', icon: HiOutlineShieldCheck },
          { name: 'Persisted Grants', to: './persisted-grants', icon: MdOutlineVerified },
          { name: 'Sessions', to: './sessions', icon: MdOutlineDevicesOther },
          { name: 'Logs', to: './logs', icon: MdOutlineHistory },
          {
            name: 'Discovery Document',
            to: `${identityServerUrl}/.well-known/openid-configuration`,
            target: '_blank',
            externalLink: true,
            icon: HiOutlineDocumentText,
          },
        ],
      }}
    >
      <Suspense
        fallback={
          <div className="h-full w-full flex items-center justify-center">
            <Spinner size="xl" />
          </div>
        }
      >
        <Outlet />
      </Suspense>
    </MainLayout>
  );
};

export const protectedRoutes = [
  {
    path: 'app',
    element: <App />,
    children: [
      { path: '/', element: <Dashboard /> },
      PersistedGrantsRoutes,
      UserSessionsRoutes,
      LogsRoutes,
      UsersRoutes,
      RolesRoutes,
      ClientsRoutes,
      IdentityResourcesRoutes,
      ApiScopesRoutes,
      ApiResourcesRoutes,
      { path: '*', element: <Dashboard /> },
    ],
  },
];
