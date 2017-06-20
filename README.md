# JSON Converter for BMPR
A simple command line utility that converts a Balsamiq Mockups Project file (\*.bmpr) to readable JSON, for quick viewing.

## What?
The **B**alsamiq **M**ockups 3 **Pr**oject file created by the desktop application (\*.bmpr) is a binary format called **B**alsamiq **Ar**chive ([BAR](https://support.balsamiq.com/resources/bmpr-format/)). This is fact an SQLite database file, with four tables INFO, RESOURCES, BRANCHES, and THUMBNAILS. Many values in these tables are JSON strings. In previous versions the project was defined as a directory containing Balsamiq Mockups Markup Language (BMML) files that were in human-readable XML format that has a lot of advantages. This utility allows enjoying same advantages of a text based format by converting BMPR to a readable JSON. 

## Why?
Using a textual format has a lot of advantages, and one of the most important one for me is being able to diff them for reviewing changes. Especially, if you manage your project files in a source control repository as I would like to. That is the main goal behind this project, but obviously there are many other benefits to using a textual format that I can't recall right now so will just say "etc" :). But I would be happy enough if I can use this utility to convert files on the fly in a diff tool of my chosing when comparing.

In this regard the main objectives are:
* Convert BMPR to a human-readable text format JSON
* Focus exclusively on readability
  * Try to match UI concepts and workflow while structuring the JSON, as compared to representing the underlying RDBMS organization
  * Try to order properties to increase readability, like having ID, name, type kind of properties at the top under an object
* Friendly to diff tools, don't rely on a lot of command line options

# Usage
```
JsonConverterBmpr.exe [-hf] SOURCE [TARGET]

    SOURCE    A *.bmpr file to convert.
    TARGET    The destination file to save the JSON output. If not give, the JSON is emitted on standard output.
    -f        Force overwrite if TARGET exists
    -h        Prints this description.
```
 
# TODO
* Simplify the awkward mockup.controls.control.children.controls.control hierarchy
  * Can possibly copy the "Children" after deserializing and then set to null so gets ignored while serializing, and remove unnecessary hierarchy from copy through a new property
  * Or can selectively parse fragments of the JSON to populate AbstractControl
* Convert all TODO and wish list items to issues
* Sort everything, like resources list etc so that they always appear in the same order

## Wish List
* Instead of emitting byte data for binary assets which is useless for readablity, start showing a hash instead so differences could be identified without bloat
* Show trashed mockups separately
* Show multiple branches of a mockup under the same mockup, instead of a separate structure
* Show names of symbols being used in a mockup, instead of ID
* Show names of images being used in a mockup, instead of ID
* Create an archive extractor, so that we can explore the mockups like a zip archive to diff tools instead of a monolithic JSON file.
* Allow editing, like for merging changes from upstream, or resolving conflict? 

# Legal

Copyright (c) 2017 Waqas Ilyas
Licenced under [MIT License](LICENSE)

Balsamiq is a trademark of Giacomo Guilizzoni, licensed to Balsamiq SRL and [Balsamiq Studios, LLC](https://balsamiq.com/termsofuse/)

This software uses third-party libraries that are distributed under their own terms, see [LICENSE-3RD-PARTY](LICENSE-3RD-PARTY.md)
