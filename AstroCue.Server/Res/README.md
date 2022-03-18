# Catalogue notes and descriptions

This file contains descriptions, filenames, sources and export options for all of the astronomical catalogues used by AstroCue.

## Table of contents
1. [Hipparcos catalogue](#hipparcos)
    1. [Hipparcos catalogue export options](#hipparcos-export)
    2. [File header](#hipparcos-header)
2. [New General Catalogue (NGC)](#ngc)
    1. [NGC export options](#ngc-export)
    2. [File header](#ngc-header)
3. [IAU Working Group on Star Names](#wgsn)
    1. [File header](#wgsn-header)
4. [NGC names catalogue](#ngc-names)
    1. [NGC names export options](#ngc-names-export)
    2. [File header](#ngc-names-header)

---
<a name="hipparcos"></a>
## [The Hipparcos and Tycho Catalogues](https://vizier.cds.unistra.fr/viz-bin/VizieR-3?-source=I/239/hip_main&-out.max=50&-out.form=HTML%20Table&-out.add=_r&-out.add=_RAJ,_DEJ&-sort=_r&-oc.form=sexa) (ESA 1997)
- **`I239_hip_main.tsv` (118218 rows)**

Sourced from Université de Strasbourg/CNRS [VizieR service](https://vizier.cds.unistra.fr/viz-bin/VizieR).
Ochsenbein F., Bauer P., Marcout J., 2000, [A&AS 143, 221](http://cdsbib.u-strasbg.fr/cgi-bin/cdsbib?2000A%26AS..143...23O)

---
<a name="hipparcos-export"></a>
### Hipparcos catalogue export settings

#### Simple constraints:
- **HIP** [`Identifier (HIP number) (H1) (meta.id;meta.main)`]
- **Vmag** [`Magnitude in Johnson V (H5) (phot.mag;em.opt.V)`]

#### Preferences:
- **max** [`unlimited`]
- **| -Separated values**
- [x] **J2000**
- [x] **Sort by Distance**
- **Position in:** [`Sexagesimal`]

---
<a name="hipparcos-header"></a>
### Hipparcos file header
```
#
#   VizieR Astronomical Server vizier.cds.unistra.fr
#    Date: 2022-02-27T15:09:07 [V1.99+ (14-Oct-2013)]
#   In case of problem, please report to:	cds-question@unistra.fr
#
#
#Coosys	J2000:	eq_FK5 J2000
#INFO	votable-version=1.99+ (14-Oct-2013)	
#INFO	-ref=VIZ621b93d3230614	
#INFO	-out.max=unlimited	
#INFO	queryParameters=15	
#-oc.form=sexa
#-out.max=unlimited
#-out.add=_r
#-out.add=_RAJ,_DEJ
#-sort=_r
#-order=I
#-out.src=I/239/hip_main
#-nav=cat:I/239&tab:{I/239/hip_main}&key:source=I/239/hip_main&HTTPPRM:&&-ref=VIZ621b93d3230614&-out.max=50&-out.form=HTML Table&-out.add=_r&-out.add=_RAJ,_DEJ&-sort=_r&-oc.form=sexa&-c.eq=J2000&-c.r=  2&-c.u=arcmin&-c.geom=r&-order=I&-out=HIP&-out=Vmag&-ignore=HIPep=*&HIPep=HIPep&Erratum=Erratum&-file=.&-meta.ucd=2&-meta=1&-meta.foot=1&-usenav=1&-bmark=POST&-out.src=I/239/hip_main
#-c.eq=J2000
#-c.r=  2
#-c.u=arcmin
#-c.geom=r
#-source=I/239/hip_main
#-out=HIP
#-out=Vmag
#

#RESOURCE=yCat_1239
#Name: I/239
#Title: The Hipparcos and Tycho Catalogues (ESA 1997)
#Coosys	H_1991.250:	ICRS
#Table	I_239_hip_main:
#Name: I/239/hip_main
#Title: The Hipparcos Main Catalogue
#Column	_RAJ2000	(A16)	Right ascension (FK5, Equinox=J2000.0) at Epoch=J2000, proper motions taken into account 	[ucd=pos.eq.ra]
#Column	_DEJ2000	(A16)	Declination (FK5, Equinox=J2000.0) at Epoch=J2000, proper motions taken into account 	[ucd=pos.eq.dec]
#Column	HIP	(I6)	Identifier (HIP number) (H1)	[ucd=meta.id;meta.main]
#Column	Vmag	(F5.2)	? Magnitude in Johnson V (H5)	[ucd=phot.mag;em.opt.V]
_RAJ2000|_DEJ2000|HIP|Vmag
"h:m:s"|"d:m:s"| |mag
```

---
<a name="ngc"></a>
## [Revised New General Catalogue](https://vizier.cds.unistra.fr/viz-bin/VizieR-3?-source=VII/1B/catalog&-out.max=50&-out.form=HTML%20Table&-out.add=_r&-out.add=_RAJ,_DEJ&-sort=_r&-oc.form=sexa) (Sulentic+, 1973) 
- **`VII_1B_catalog.tsv` (8163 rows)**

Sourced from Université de Strasbourg/CNRS [VizieR service](https://vizier.cds.unistra.fr/viz-bin/VizieR).
Ochsenbein F., Bauer P., Marcout J., 2000, [A&AS 143, 221](http://cdsbib.u-strasbg.fr/cgi-bin/cdsbib?2000A%26AS..143...23O)

---
<a name="ngc-export"></a>
### NGC export settings

#### Simple constraints:
- **NGC** [`[1/7840] The original NGC number (meta.id;meta.main)`]
- **Type** [`Type of object (Note 1)   (src.class)`]
- **Mag** [`Magnitude rounded to the nearest half magnitude (phot.mag)`]

#### Preferences:
- **max** [`unlimited`]
- **| -Separated values**
- [x] **J2000**
- [x] **Sort by Distance**
- **Position in:** [`Sexagesimal`]

---
<a name="ngc-header"></a>
### NGC file header
```
#
#   VizieR Astronomical Server vizier.cfa.harvard.edu
#    Date: 2022-02-27T16:39:18 [V1.99+ (14-Oct-2013)]
#   In case of problem, please report to:	cds-question@unistra.fr
#
#
#Coosys	J2000:	eq_FK5 J2000
#INFO	votable-version=1.99+ (14-Oct-2013)	
#INFO	-ref=VIZ621ba8ef50dd	
#INFO	-out.max=unlimited	
#INFO	queryParameters=17	
#-oc.form=sexa
#-out.max=unlimited
#-out.add=_r
#-out.add=_RAJ,_DEJ
#-sort=_r
#-order=I
#-out.src=VII/1B/catalog
#-nav=cat:VII/1B&tab:{VII/1B/catalog}&key:source=VII/1B/catalog&HTTPPRM:&&-ref=VIZ621ba8ef50dd&-out.max=50&-out.form=HTML Table&-out.add=_r&-out.add=_RAJ,_DEJ&-sort=_r&-oc.form=sexa&-c.eq=J2000&-c.r=  2&-c.u=arcmin&-c.geom=r&-order=I&-out=NGC&-out=m_NGC&-out=Type&-out=Mag&-file=.&-meta.ucd=2&-meta=1&-meta.foot=1&-usenav=1&-bmark=POST&-out.src=VII/1B/catalog
#-c.eq=J2000
#-c.r=  2
#-c.u=arcmin
#-c.geom=r
#-source=VII/1B/catalog
#-out=NGC
#-out=m_NGC
#-out=Type
#-out=Mag
#

#RESOURCE=yCat_7001
#Name: VII/1B
#Title: Revised New General Catalogue (Sulentic+, 1973)
#Coosys	B1975:	eq_FK4 B1975
#Table	VII_1B_catalog:
#Name: VII/1B/catalog
#Title: The Revised NGC Catalog
#Column	_RAJ2000	(A10)	Right ascension (FK5, Equinox=J2000.0) (computed by VizieR, not part of the original data. The format may include more digits than the original data because of internal accuracy requirements in VizieR and across other CDS services)	[ucd=pos.eq.ra]
#Column	_DEJ2000	(A9)	Declination (FK5, Equinox=J2000.0) (computed by VizieR, not part of the original data. The format may include more digits than the original data because of internal accuracy requirements in VizieR and across other CDS services)	[ucd=pos.eq.dec]
#Column	NGC	(I4)	[1/7840]+= The original NGC number	[ucd=meta.id;meta.main]
#Column	m_NGC	(A1)	Component of NGC (6)	[ucd=meta.code.multip]
#Column	Type	(I2)	Type of object (1)	[ucd=src.class]
#Column	Mag	(F4.1)	? Magnitude rounded to the nearest half magnitude	[ucd=phot.mag]
_RAJ2000|_DEJ2000|NGC|m_NGC|Type|Mag
"h:m:s"|"d:m:s"| | | |mag
```

---
<a name="wgsn"></a>
## [International Astronomical Union Working Group on Star Names](http://www.pas.rochester.edu/~emamajek/WGSN/IAU-CSN.txt) (WGSN) 
- **`I239_hip_main_names.tsv` (449 rows)**

https://www.iau.org/science/scientific_bodies/working_groups/280/

---
<a name="wgsn-header"></a>
### WGSN file header
```
# "IAU Catalog of Star Names (IAU-CSN)"
# IAU Division C Working Group on Star Names (WGSN)
# https://www.iau.org/science/scientific_bodies/working_groups/280/
# WGSN Chair: Eric Mamajek. Email questions or comments to: IAUWGSN@gmail.com
# Last updated 2021-01-01 (see notes on latest edits at end of file)
#
# For use of the contents of the data of this file, we encourage users
# to refer to the (identical) official IAU version of the data at:
# https://www.iau.org/public/themes/naming_stars/. At times there may
# be a time delay between the posting of names in this file and what
# is posted on the IAU website. All IAU-produced products (Images,
# Videos, Texts) are released under Creative Commons Attribution
# (i.e. free to use in all perpetuity, world-wide, as long as the
# source is mentioned). Rows (2) and (5) require UTF-8 encoding (For
# Firefox browser, set > View > Text Encoding > Unicode).  WGSN is
# working to add brief summaries of etymological information to future
# editions of this table.
#
#(1)              (2)               (3)          (4)   (5)   (6) (7)  (8)         (9)    (10)   (11) (12)       (13)       (14)       (15)
#Name/ASCII       Name/Diacritics   Designation  ID    ID    Con #    WDS_J       Vmag    HIP     HD RA(J2000)  Dec(J2000) Date       Notes
```

```
#Name/ASCII       Name/Diacritics   Designation  ID    ID    Con #    WDS_J       Vmag    HIP     HD RA(J2000)  Dec(J2000) Date       Notes
#(1)              (2)               (3)          (4)   (5)   (6) (7)  (8)         (9)    (10)   (11) (12)       (13)       (14)       (15)
#
# Notes:
#  (1) Proper name adopted by IAU WGSN (ASCII version)
#  (2) Proper name adopted by IAU Working Group on Star Names (WGSN)
#      (version with diacritic symbols, UTF-8).
#  (3) Fiducial designation (in order of preference and availability):
#      HR, GJ, HD, HIP, or discovery survey designation for host stars
#      of transiting exoplanets (see SIMBAD;
#      http://simbad.u-strasbg.fr/simbad/).
#  (4) ID column is either (in order of preference and availability)
#      Bayer greek letter (using SIMBAD databases; e.g. "alf" = alpha; see
#      table http://simbad.u-strasbg.fr/Pages/guide/chA.htx), Flamsteed
#      numbers (both following The Bright Star Catalogue [BSC], 5th Revised
#      Ed. (Hoffleit & Warren 1991), or variable star designations.
#  (5) ID, same as column 3 but Bayer Greek letters in UTF-8 encoding. 
#  (6) Constellation (3-letter IAU abbrev.; following "The IAU Style Manual",
#      Wilkins 1989, Table 11).
#  (7) The "#" after the constellation column is the component ID to
#      Washington Double Star (WDS) catalog multiple system, if needed
#      (e.g. Proxima Centauri = alf Cen "C"), but may be left blank
#      ("_") where the primary, by visual brightness, is unambiguous
#      (blank = A) or if the other WDS catalogue entries are obviously
#      unphysical (i.e. the star appears to have no known physical
#      stellar companions). Component IDs are also given where the
#      star does not have a WDS entry (i.e. is not a resolved
#      multiple), but the star is either known to be a spectroscopic
#      binary or astrometric binary in the literature (usually a "A"
#      or "Aa" is given in those cases, e.g. Wezen). Many thanks to
#      Brian Mason (IAU Commission G1 Binary and Multiple Star
#      Systems) for help with vetting the WDS designations and IDs.
#  (8) WDS designation for multiple systems (or candidate multiple systems),
#      the official double star catalogue of IAU Commission 26 (now 
#  (9) Magnitude in Johnson V photometric band. Magnitudes are preferentially
#      from The Hipparcos and Tycho catalogues (ESA 1997), WDS, or Gaia DR2
#      catalog. For fainter stars, they are calculated from Gaia EDR3
#      G magnitude and Bp-Rp color following Riello+2020. 
# (10) HIP designations from The Hipparcos and Tycho Catalogues (ESA 1997).
# (11) HD designations, usually cross-identified from SIMBAD, BSC, HIP
#      catalogues. 
# (12) Right Ascension (ICRS, epoch 2000.0)
# (13) Declination (ICRS, epoch 2000.0) Columns 10 and 11 are
#      calculated for epoch 2000.0 using Vizier, usually using
#      positions and proper motions from the revised Hipparcos
#      catalogue ("Hipparcos, the New Reduction of the Raw Data" (van
#      Leeuwen 2007 A&A, 474, 653), Gaia DR2 (Gaia Collaboration et
#      al.  2018 A&A, 616, A1), or Tycho-2 catalogue (Hog et al. 2000,
#      A&A, 355, L27).
# (14) Date approved. Names marked approved "2015-12-15" are exoplanet
#      host star names - a mix of common names and names adopted via
#      the NameExoWorlds contest - reviewed and adopted by the
#      Executive Committee WG Public Naming of Planets and Planetary
#      Satellites, and recognized by WGSN (although the posted date
#      pre-dates WGSN).  Names dated "2019-12-17" are exoplanet host
#      star names from the IAU100 NameExoWorlds public naming
#      campaign, and the winning entries were reviewed and adopted by
#      the IAU100 NameExoWorlds Steering Committee (which had
#      representatives from WGSN).
#
# (15) Notes: 
#      * Acamar is tet01 Eri = tet Eri A = WDS J02583-4018A.
#      * Algieba is gam01 Leo = gam Leo A = WDS J10200+1950A.
#      * Alcor is 80 UMa A, which is component Ca of multiple WDS
#        J13239+5456 (Mizar is component Aa).
#      * Almach is gam01 And = gam And A = WDS J02039+4220A.
#      * Pipirima is mu02 Sco (WDS J16523-3801A) and Xamidimura is mu01 Sco
#        Aa (WDS J16519-3803Aa) and, however the these two stars are a
#        wide physical multiple (WDS J16519-3803AD; sep. 346.8").
#      * alf Cen system: three stellar components have IAU names:
#        A: Rigil Kentaurus, B: Toliman, C: Proxima Centauri. 
#
# Notes on edits. 
# 2018-06-11: Piautos: An incorrect position for Piautos was listed on a
#             version of this list posted in early June 2018. The position
#             has been corrected.
# 2018-06-19: The WDS and component entries for Polaris and Polaris
#             Australis were flipped and this has been corrected.
#             Polaris is WDS J02318+8916 Aa, while Polaris Australis
#             has no WDS entry nor component ID.
# 2018-07-09: Okab: Typo in HIP # for Okab corrected to HIP 93747.
# 2018-08-10: Algedi: removed WDS component ID as none of the WDS
#             entries appear to be physically related. 
# 2018-08-10: Azmidi: removed WDS component ID as B is unlikely to
#             be physically connected with A, and evidence for SB
#             status of A is weak.
# 2018-08-10: Added: Gudja, Guniibuu, Imai, La Superba, Paikauhale & Toliman.
# 2018-09-07: Fixed: Cujam: fixed o Her to ω Her (rest of data was correct).
# 2019-09-20: Fixed: Piautos: added A component as system is an
#             unresolved spectroscopic binary (Gullikson et al., 2016,
#             AJ, 152, 40).
# 2020-02-13: Added: 112 star names from IAU100 NameExoWorlds public
#             naming campaign (see names, etymologies, and names of
#             proposers at:
#             http://www.nameexoworlds.iau.org/final-results). Note
#             that the spelling of the name for HD 145457 from the
#             Japan IAU100 NameExoWorlds campaign was originally
#             "Kamui", but after consultation with the Hokkaido Ainu
#             association, the IAU100 NameExoWorlds Steering Committee
#             approved amending the spelling to "Kamuy".
# 2020-03-01: Added: Mazaalai = HAT-P-21. Mazaalai is the winning
#             entry from Mongolia, and the last star name approved by
#             the IAU100 NameExoWorlds Steering Committee for the 2019
#             public naming campaign (exoplanet HAT-P-21b was assigned
#             name Bambaruush).
# 2020-03-15: Several NameExoWorlds 2019 star names were found to be
#             misassigned to stars due to sorting errors (names from
#             Denmark(Muspelheim), Dominican Republic(Marohu),
#             Finland(Horna), Ireland(Tuiren), Malta(Sansuna),
#             Pakistan(Shama), Palestine(Moriah), Russia(Dombay),
#             Serbia(Morava), South Africa(Naledi)).  The
#             name-designation assignments have been checked again
#             with: http://www.nameexoworlds.iau.org/final-results.
#             If you used the 2020-02-13 or 2020-03-01 versions,
#             please update to this latest version. Thanks to Eduardo
#             Penteado for pointing out the errors.
# 2021-01-01: Column for photometric band was removed as all
#             magnitudes are now Johnson V-band. V-band magnitudes
#             were calculated for the following stars from Gaia EDR3 G
#             magnitude and Bp-Rp color following Riello+2020:
#             Absolutno, Anadolu, Atakoraka, Berehynia, Chaophraya,
#             Chason, Diwo, Diya, Dombay, Gloas, Horna, Koit, Lerna,
#             Malmok, Marohu, Mazaalai, Moriah, Mpingo, Muspelheim,
#             Nyamien, Parumleo, Petra, Pipoltr, Tangra, Tevel, Tojil,
#             and Tuiren. Added WDS IDs for Diya, Terebellum,
#             Zubeneschamali.
```

---
<a name="ngc-names"></a>
## NGC 2000.0 (Sky Publishing, ed. Sinnott 1988) [Index of Messier and common names](https://vizier.cds.unistra.fr/viz-bin/VizieR-3?-source=VII/118/names&-out.max=50&-out.form=HTML%20Table&-out.add=_r&-out.add=_RAJ,_DEJ&-sort=_r&-oc.form=sexa)
- **`VII_1B_catalog_names.tsv_` (227 rows)**

Sourced from Université de Strasbourg/CNRS [VizieR service](https://vizier.cds.unistra.fr/viz-bin/VizieR).
Ochsenbein F., Bauer P., Marcout J., 2000, [A&AS 143, 221](http://cdsbib.u-strasbg.fr/cgi-bin/cdsbib?2000A%26AS..143...23O)

---
<a name="ngc-names-export"></a>
### Index of Messier and common names export options

#### Simple constraints:
- **Object** [`Common name (including Messier numbers) (meta.id;meta.main)`]
- **Name** [`NGC or IC name, as in ngc2000.dat (Note)   (meta.id)`]

#### Preferences:
- **max** [`unlimited`]
- **| -Separated values**
- [x] **Sort by Distance**
- **Position in:** [`Sexagesimal`]

---
<a name="ngc-names-header"></a>
### Index of Messier and common names file header
```
#
#   VizieR Astronomical Server vizier.cds.unistra.fr
#    Date: 2022-02-28T17:32:17 [V1.99+ (14-Oct-2013)]
#   In case of problem, please report to:	cds-question@unistra.fr
#
#
#Coosys	J2000:	eq_FK5 J2000
#INFO	votable-version=1.99+ (14-Oct-2013)	
#INFO	-ref=VIZ621d01cc16b6cc	
#INFO	-out.max=unlimited	
#INFO	queryParameters=9	
#-oc.form=dec
#-out.max=unlimited
#-sort=_r
#-order=I
#-out.src=VII/118/names
#-nav=cat:VII/118&tab:{VII/118/names}&key:source=VII/118/names&HTTPPRM:&&-ref=VIZ621d01cc16b6cc&-out.max=50&-out.form=HTML Table&-out.add=_r&-out.add=_RAJ,_DEJ&-sort=_r&-oc.form=sexa&-order=I&-out=Object&-out=Name&-file=.&-meta.ucd=2&-meta=1&-meta.foot=1&-usenav=1&-bmark=POST&-out.src=VII/118/names
#-source=VII/118/names
#-out=Object
#-out=Name
#

#RESOURCE=yCat_7118
#Name: VII/118
#Title: NGC 2000.0 (Sky Publishing, ed. Sinnott 1988)
#Table	VII_118_names:
#Name: VII/118/names
#Title: Index of Messier and common names
#Column	Object	(a35)	Common name (including Messier numbers)	[ucd=meta.id;meta.main]
#Column	Name	(A5)	NGC or IC name, as in ngc2000.dat	[ucd=meta.id]
Object|Name
```
---