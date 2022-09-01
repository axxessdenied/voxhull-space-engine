/*
/ Created   : 4/14/2020 2:54:40 PM
/ Script    : C.cs - Constants
/ Author    : Nick Slusarczyk
/ Company   : Chernobog.Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chernobog.Studio.Common;

    public static class C
    {
        public const double G = 6.67430e-11;
        public const float G_F = 6.67384f;
        public const double PI = 3.1415926535897932384626433;
        public const double TWO_PI = 6.2831853071795864769252866;
        public const double DEG2RAD = 0.017453292519943;
        public const double RAD2DEG = 57.295779513082;
        public const double EPSILON = 1.401298E-45;

        
        // Machine constants
        public const double MACHEP = 1.11022302462515654042E-16;
        public const double MAXLOG = 7.09782712893383996732E2;
        public const double MINLOG = -7.451332191019412076235E2;
        public const double MAXGAM = 171.624376956302725;
        public const double SQTPI = 2.50662827463100050242E0;
        public const double SQRTH = 7.07106781186547524401E-1;
        public const double LOGPI = 1.14472988584940017414;


        // Physical Constants in cgs Units

        /// <summary>
        /// Boltzman Constant. Units erg/deg(K) 
        /// </summary>
        public const double BOLTZMAN = 1.3807e-16;

        /// <summary>
        /// Elementary Charge. Units statcoulomb 
        /// </summary>
        public const double ECHARGE = 4.8032e-10;

        /// <summary>
        /// Electron Mass. Units g 
        /// </summary>
        public const double EMASS = 9.1095e-28;

        /// <summary>
        /// Proton Mass. Units g 
        /// </summary>
        public const double PMASS = 1.6726e-24;

        /// <summary>
        /// Gravitational Constant. Units dyne-cm^2/g^2
        /// </summary>
        public const double GRAV = 6.6720e-08;

        /// <summary>
        /// Planck constant. Units erg-sec 
        /// </summary>
        public const double PLANCK = 6.6262e-27;

        /// <summary>
        /// Speed of Light in a Vacuum. Units cm/sec 
        /// </summary>
        public const double LIGHTSPEED = 2.9979e10;

        /// <summary>
        /// Stefan-Boltzman Constant. Units erg/cm^2-sec-deg^4 
        /// </summary>
        public const double STEFANBOLTZ = 5.6703e-5;

        /// <summary>
        /// Avogadro Number. Units  1/mol 
        /// </summary>
        public const double AVOGADRO = 6.0220e23;

        /// <summary>
        /// Gas Constant. Units erg/deg-mol 
        /// </summary>
        public const double GASCONSTANT = 8.3144e07;

        /// <summary>
        /// Gravitational Acceleration at the Earths surface. Units cm/sec^2 
        /// </summary>
        public const double GRAVACC = 980.67;

        /// <summary>
        /// Solar Mass. Units g 
        /// </summary>
        public const double SOLARMASS = 1.99e33;

        /// <summary>
        /// Solar Radius. Units cm
        /// </summary>
        public const double SOLARRADIUS = 6.96e10;

        /// <summary>
        /// Solar Luminosity. Units erg/sec
        /// </summary>
        public const double SOLARLUM = 3.90e33;

        /// <summary>
        /// Solar Flux. Units erg/cm^2-sec
        /// </summary>
        public const double SOLARFLUX = 6.41e10;

        /// <summary>
        /// Astronomical Unit (radius of the Earth's orbit). Units cm
        /// </summary>
        public const double AU = 1.50e13;
    }
}