using System;

namespace FitnessTrackerApp
{
    public static class SessionManager
    {
        // Stores the username of the currently logged-in user
        public static string CurrentUsername { get; set; }

        // Stores the time when the user logged in
        public static DateTime LoginTime { get; set; }

        // Clears the session data (used for logout)
        public static void ClearSession()
        {
            CurrentUsername = null;
        }
    }
}